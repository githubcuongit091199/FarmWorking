using FarmWorking.Infrastructure.Persistence;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D = FarmWorking.Domain.Entities;

namespace FarmWorking.Api.Controllers;

[ApiController, Route("api/knowledge")]
public class KnowledgeController(FarmDbContext db, IWebHostEnvironment environment) : ControllerBase
{
    private const long MaxFileSize = 20 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".csv" };

    [HttpGet]
    public ActionResult<IEnumerable<KnowledgeItem>> List([FromQuery] Guid? parentId = null) => Ok(db.KnowledgeItems.AsNoTracking()
        .Where(x=>x.ParentId==parentId).OrderByDescending(x=>x.IsFolder).ThenBy(x=>x.Name).Select(ToContract).ToList());

    [HttpPost("folders")]
    public async Task<ActionResult<KnowledgeItem>> CreateFolder(CreateKnowledgeFolderRequest request,CancellationToken ct)
    {
        var name=CleanName(request.Name);if(name.Length==0)return BadRequest("Tên thư mục không được để trống.");
        if(request.ParentId.HasValue&&!await db.KnowledgeItems.AnyAsync(x=>x.Id==request.ParentId&&x.IsFolder,ct))return NotFound("Thư mục cha không tồn tại.");
        if(await db.KnowledgeItems.AnyAsync(x=>x.ParentId==request.ParentId&&x.Name==name,ct))return Conflict("Tên đã tồn tại trong thư mục này.");
        var item=new D.KnowledgeItem{Id=Guid.NewGuid(),ParentId=request.ParentId,IsFolder=true,Name=name};db.KnowledgeItems.Add(item);await db.SaveChangesAsync(ct);return Ok(ToContract(item));
    }

    [HttpPost("files"),RequestSizeLimit(MaxFileSize)]
    public async Task<ActionResult<KnowledgeItem>> Upload([FromForm]IFormFile file,[FromForm]Guid? parentId,CancellationToken ct)
    {
        if(file.Length==0||file.Length>MaxFileSize)return BadRequest("Tệp phải có dung lượng từ 1 byte đến 20 MB.");
        var extension=Path.GetExtension(file.FileName);if(!AllowedExtensions.Contains(extension))return BadRequest("Loại tệp không được hỗ trợ.");
        if(parentId.HasValue&&!await db.KnowledgeItems.AnyAsync(x=>x.Id==parentId&&x.IsFolder,ct))return NotFound("Thư mục không tồn tại.");
        var name=CleanName(Path.GetFileName(file.FileName));if(await db.KnowledgeItems.AnyAsync(x=>x.ParentId==parentId&&x.Name==name,ct))return Conflict("Tên tệp đã tồn tại trong thư mục này.");
        var stored=$"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";var directory=StorageDirectory();Directory.CreateDirectory(directory);
        await using(var output=System.IO.File.Create(Path.Combine(directory,stored)))await file.CopyToAsync(output,ct);
        var item=new D.KnowledgeItem{Id=Guid.NewGuid(),ParentId=parentId,Name=name,StoredName=stored,ContentType=file.ContentType,Size=file.Length};db.KnowledgeItems.Add(item);await db.SaveChangesAsync(ct);return Ok(ToContract(item));
    }

    [HttpGet("{id:guid}/content")]
    public IActionResult Content(Guid id,[FromQuery]bool download=false){var item=db.KnowledgeItems.AsNoTracking().FirstOrDefault(x=>x.Id==id&&!x.IsFolder);if(item is null)return NotFound();var path=Path.Combine(StorageDirectory(),item.StoredName);if(!System.IO.File.Exists(path))return NotFound();return PhysicalFile(path,string.IsNullOrWhiteSpace(item.ContentType)?"application/octet-stream":item.ContentType,download?item.Name:null);}

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<KnowledgeItem>> Rename(Guid id,RenameKnowledgeItemRequest request,CancellationToken ct){var item=await db.KnowledgeItems.FindAsync([id],ct);if(item is null)return NotFound();var name=CleanName(request.Name);if(name.Length==0)return BadRequest("Tên không được để trống.");if(await db.KnowledgeItems.AnyAsync(x=>x.Id!=id&&x.ParentId==item.ParentId&&x.Name==name,ct))return Conflict("Tên đã tồn tại.");item.Name=name;await db.SaveChangesAsync(ct);return Ok(ToContract(item));}

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id,CancellationToken ct){var item=await db.KnowledgeItems.FindAsync([id],ct);if(item is null)return NotFound();await DeleteTree(item,ct);await db.SaveChangesAsync(ct);return NoContent();}

    private async Task DeleteTree(D.KnowledgeItem item,CancellationToken ct){foreach(var child in await db.KnowledgeItems.Where(x=>x.ParentId==item.Id).ToListAsync(ct))await DeleteTree(child,ct);if(!item.IsFolder){var path=Path.Combine(StorageDirectory(),item.StoredName);if(System.IO.File.Exists(path))System.IO.File.Delete(path);}db.KnowledgeItems.Remove(item);}
    private string StorageDirectory()=>Path.Combine(environment.ContentRootPath,"App_Data","knowledge");
    private static string CleanName(string value)=>string.Join("_",value.Trim().Split(Path.GetInvalidFileNameChars(),StringSplitOptions.RemoveEmptyEntries));
    private static KnowledgeItem ToContract(D.KnowledgeItem x)=>new(){Id=x.Id,ParentId=x.ParentId,IsFolder=x.IsFolder,Name=x.Name,ContentType=x.ContentType,Size=x.Size,CreatedAt=x.CreatedAt};
}
