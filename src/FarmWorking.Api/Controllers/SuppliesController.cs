using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmWorking.Infrastructure.Persistence;
using E = FarmWorking.Domain.Enums;

namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/supplies")]
public class SuppliesController(ISupplyService service, IWebHostEnvironment environment, FarmDbContext db) : ControllerBase
{
    private static readonly Dictionary<string, string> AllowedImages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image/jpeg"] = ".jpg", ["image/png"] = ".png", ["image/webp"] = ".webp", ["image/gif"] = ".gif"
    };

    [HttpGet] public ActionResult<IEnumerable<Supply>> GetAll(SupplyType? type = null) => Ok(service.GetAll(type is null ? null : (E.SupplyType)type).Select(x => x.ToContract()));
    [HttpGet("{id:guid}")] public ActionResult<Supply> Get(Guid id) { var result = service.GetById(id); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpPost] public async Task<ActionResult<Supply>> Create(Supply item, CancellationToken ct) { var result = (await service.CreateAsync(item.ToDomain(), ct)).ToContract(); return CreatedAtAction(nameof(Get), new { id = result.Id }, result); }
    [HttpPut("{id:guid}")] public async Task<ActionResult<Supply>> Update(Guid id, Supply item, CancellationToken ct) { var result = await service.UpdateAsync(id, item.ToDomain(), ct); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        if (await db.FarmSupplyEntries.AnyAsync(x => x.SupplyId == id || x.SupplyPrice.SupplyId == id, ct))
            return Conflict("Vật tư đã có lịch sử nhập kho nên không thể xóa. Bạn vẫn có thể sửa thông tin vật tư.");

        // SupplyPrices are deleted atomically by the cascading foreign key.
        return await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/prices")]
    public async Task<ActionResult<IEnumerable<FarmWorking.Shared.SupplyPrice>>> Prices(Guid id,CancellationToken ct)=>Ok(await db.SupplyPrices.AsNoTracking().Where(x=>x.SupplyId==id).OrderByDescending(x=>x.CreatedAt).Select(x=>new FarmWorking.Shared.SupplyPrice{Id=x.Id,SupplyId=x.SupplyId,Price=x.Price,CreatedAt=x.CreatedAt}).ToListAsync(ct));

    [HttpPost("{id:guid}/prices")]
    public async Task<ActionResult<FarmWorking.Shared.SupplyPrice>> AddPrice(Guid id,AddSupplyPriceRequest request,CancellationToken ct)
    {
        if(request.Price<0)return BadRequest("Giá không được âm.");if(!await db.Supplies.AnyAsync(x=>x.Id==id,ct))return NotFound();
        if(await db.SupplyPrices.AnyAsync(x=>x.SupplyId==id&&x.Price==request.Price,ct))return BadRequest("Mức giá này đã tồn tại.");
        var x=new FarmWorking.Domain.Entities.SupplyPrice{SupplyId=id,Price=request.Price};db.SupplyPrices.Add(x);await db.SaveChangesAsync(ct);return Ok(new FarmWorking.Shared.SupplyPrice{Id=x.Id,SupplyId=x.SupplyId,Price=x.Price,CreatedAt=x.CreatedAt});
    }

    [HttpDelete("prices/{priceId:guid}")]
    public async Task<IActionResult> DeletePrice(Guid priceId,CancellationToken ct){var x=await db.SupplyPrices.FindAsync([priceId],ct);if(x is null)return NotFound();if(await db.FarmSupplyEntries.AnyAsync(e=>e.SupplyPriceId==priceId,ct))return BadRequest("Mức giá đã được dùng trong lịch sử nhập kho nên không thể xóa.");db.SupplyPrices.Remove(x);await db.SaveChangesAsync(ct);return NoContent();}


    [HttpPost("image")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadImage(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("Tệp ảnh trống.");
        if (file.Length > 5 * 1024 * 1024) return BadRequest("Ảnh không được vượt quá 5 MB.");
        if (!AllowedImages.TryGetValue(file.ContentType, out var extension))
            return BadRequest("Chỉ hỗ trợ JPG, PNG, WEBP hoặc GIF.");

        var uploadDirectory = Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "supplies");
        Directory.CreateDirectory(uploadDirectory);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using var output = System.IO.File.Create(Path.Combine(uploadDirectory, fileName));
        await file.CopyToAsync(output, ct);
        return Ok(new { url = $"{Request.Scheme}://{Request.Host}/uploads/supplies/{fileName}" });
    }

}
