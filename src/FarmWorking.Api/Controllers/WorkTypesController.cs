using FarmWorking.Infrastructure.Persistence;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmWorking.Api.Controllers;
[ApiController,Route("api/work-types")]
public class WorkTypesController(FarmDbContext db):ControllerBase
{
    [HttpGet]public async Task<ActionResult<IEnumerable<WorkType>>> Get(CancellationToken ct)=>Ok(await db.WorkTypes.AsNoTracking().OrderBy(x=>x.Name).Select(x=>new WorkType{Id=x.Id,Name=x.Name,DailyRate=x.DailyRate,IsActive=x.IsActive}).ToListAsync(ct));
    [HttpPost]public async Task<ActionResult<WorkType>> Create(WorkType item,CancellationToken ct){if(string.IsNullOrWhiteSpace(item.Name)||item.DailyRate<0)return BadRequest("Tên công việc và đơn giá không hợp lệ.");if(await db.WorkTypes.AnyAsync(x=>x.Name==item.Name.Trim(),ct))return BadRequest("Công việc đã tồn tại.");var x=new FarmWorking.Domain.Entities.WorkType{Name=item.Name.Trim(),DailyRate=item.DailyRate,IsActive=item.IsActive};db.WorkTypes.Add(x);await db.SaveChangesAsync(ct);item.Id=x.Id;return Ok(item);}
    [HttpPut("{id:guid}")]public async Task<IActionResult> Update(Guid id,WorkType item,CancellationToken ct){var x=await db.WorkTypes.FindAsync([id],ct);if(x is null)return NotFound();if(string.IsNullOrWhiteSpace(item.Name)||item.DailyRate<0)return BadRequest("Tên công việc và đơn giá không hợp lệ.");if(await db.WorkTypes.AnyAsync(y=>y.Id!=id&&y.Name==item.Name.Trim(),ct))return BadRequest("Công việc đã tồn tại.");x.Name=item.Name.Trim();x.DailyRate=item.DailyRate;x.IsActive=item.IsActive;await db.SaveChangesAsync(ct);return Ok();}
    [HttpDelete("{id:guid}")]public async Task<IActionResult> Delete(Guid id,CancellationToken ct){var x=await db.WorkTypes.FindAsync([id],ct);if(x is null)return NotFound();if(await db.WorkerWorkDays.AnyAsync(y=>y.WorkTypeId==id,ct))return BadRequest("Công việc đã được tính công nên không thể xóa; hãy chuyển sang ngừng sử dụng.");db.WorkTypes.Remove(x);await db.SaveChangesAsync(ct);return NoContent();}
}
