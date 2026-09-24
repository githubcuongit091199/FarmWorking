using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmWorking.Infrastructure.Persistence;

namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/farms")]
public class FarmsController(IFarmService service, FarmDbContext db) : ControllerBase
{
    [HttpGet] public ActionResult<IEnumerable<Farm>> GetAll() => Ok(service.GetAll().Select(x => x.ToContract()));
    [HttpGet("{id:guid}")] public ActionResult<FarmDetail> Get(Guid id) { var x = service.GetById(id); return x is null ? NotFound() : Ok(new FarmDetail { Farm = x.Farm.ToContract(), Notes = x.Notes.Select(y => y.ToContract()).ToList(), Transactions = x.Transactions.Select(y => y.ToContract()).ToList(), Processes = x.Processes.Select(y => y.ToContract()).ToList(), ExtraSteps = x.ExtraSteps.Select(y => y.ToContract()).ToList(), ProcessRuns=x.ProcessRuns.Select(y=>y.ToContract()).ToList() }); }
    [HttpPost] public async Task<ActionResult<Farm>> Create(Farm farm, CancellationToken ct) { var result = (await service.CreateAsync(farm.ToDomain(), ct)).ToContract(); return CreatedAtAction(nameof(Get), new { id = result.Id }, result); }
    [HttpPut("{id:guid}")] public async Task<ActionResult<Farm>> Update(Guid id, Farm farm, CancellationToken ct) { var result = await service.UpdateAsync(id, farm.ToDomain(), ct); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id, CancellationToken ct) => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
    [HttpPost("{id:guid}/notes")] public async Task<ActionResult<WorkNote>> AddNote(Guid id, WorkNote note, CancellationToken ct) { var result = await service.AddNoteAsync(id, note.ToDomain(), ct); return result is null ? NotFound() : Created($"/api/farms/{id}/notes/{result.Id}", result.ToContract()); }
    [HttpGet("{id:guid}/notes/{noteId:guid}")] public ActionResult<WorkNote> GetNote(Guid id, Guid noteId) { var result = service.GetNote(id, noteId); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpPut("{id:guid}/notes/{noteId:guid}")] public async Task<ActionResult<WorkNote>> UpdateNote(Guid id, Guid noteId, WorkNote note, CancellationToken ct) { var result = await service.UpdateNoteAsync(id, noteId, note.ToDomain(), ct); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpDelete("{id:guid}/notes/{noteId:guid}")] public async Task<IActionResult> DeleteNote(Guid id, Guid noteId, CancellationToken ct) => await service.DeleteNoteAsync(id, noteId, ct) ? NoContent() : NotFound();
    [HttpPost("{id:guid}/processes/{processId:guid}/extra-steps")] public async Task<ActionResult<FarmProcessExtraStep>> AddExtraStep(Guid id, Guid processId, FarmProcessExtraStep step, CancellationToken ct) { try { var result=await service.AddExtraStepAsync(id,processId,step.ToDomain(),ct);return result is null?NotFound():Ok(result.ToContract()); } catch(ArgumentException ex){return BadRequest(ex.Message);} }
    [HttpDelete("{id:guid}/processes/{processId:guid}/extra-steps/{stepId:guid}")] public async Task<IActionResult> DeleteExtraStep(Guid id, Guid processId, Guid stepId, CancellationToken ct)=>await service.DeleteExtraStepAsync(id,processId,stepId,ct)?NoContent():NotFound();
    [HttpPost("{id:guid}/processes/{processId:guid}/runs")] public async Task<ActionResult<FarmProcessRun>> StartRun(Guid id,Guid processId,StartFarmProcessRunRequest request,CancellationToken ct){try{var result=await service.StartProcessRunAsync(id,processId,request.SeasonName,request.StartDate,ct);return result is null?NotFound():Ok(result.ToContract());}catch(ArgumentException ex){return BadRequest(ex.Message);}}
    [HttpPut("{id:guid}/runs/{runId:guid}/steps/{stepId:guid}")] public async Task<ActionResult<FarmProcessRun>> CompleteRunStep(Guid id,Guid runId,Guid stepId,[FromQuery]bool completed,CancellationToken ct){var result=await service.SetRunStepCompletedAsync(id,runId,stepId,completed,ct);return result is null?NotFound():Ok(result.ToContract());}
    [HttpDelete("{id:guid}/runs/{runId:guid}")] public async Task<IActionResult> DeleteRun(Guid id,Guid runId,CancellationToken ct)=>await service.DeleteProcessRunAsync(id,runId,ct)?NoContent():NotFound();

    [HttpGet("{id:guid}/supplies")]
    public async Task<ActionResult<IEnumerable<FarmWorking.Shared.FarmSupplyEntry>>> Supplies(Guid id,CancellationToken ct)=>Ok(await db.FarmSupplyEntries.AsNoTracking().Where(x=>x.FarmId==id).Include(x=>x.Supply).OrderByDescending(x=>x.ReceivedAt).Select(x=>new FarmWorking.Shared.FarmSupplyEntry{Id=x.Id,FarmId=x.FarmId,SupplyId=x.SupplyId,SupplyPriceId=x.SupplyPriceId,SupplyName=x.Supply.Name,SupplyType=(SupplyType)x.Supply.Type,Unit=x.Supply.Unit,UnitPrice=x.UnitPrice,Quantity=x.Quantity,UsedQuantity=x.UsedQuantity,ReceivedAt=x.ReceivedAt}).ToListAsync(ct));

    [HttpPost("{id:guid}/supplies")]
    public async Task<IActionResult> ProvisionSupply(Guid id,ProvisionFarmSupplyRequest request,CancellationToken ct)
    {
        if(request.Quantity<=0)return BadRequest("Số lượng nhập phải lớn hơn 0.");if(!await db.Farms.AnyAsync(x=>x.Id==id,ct))return NotFound();
        var price=await db.SupplyPrices.FirstOrDefaultAsync(x=>x.Id==request.SupplyPriceId&&x.SupplyId==request.SupplyId,ct);if(price is null)return BadRequest("Mức giá không thuộc vật tư đã chọn.");
        db.FarmSupplyEntries.Add(new FarmWorking.Domain.Entities.FarmSupplyEntry{FarmId=id,SupplyId=request.SupplyId,SupplyPriceId=price.Id,UnitPrice=price.Price,Quantity=request.Quantity,ReceivedAt=request.ReceivedAt.Date});await db.SaveChangesAsync(ct);return Ok();
    }

    [HttpPost("{id:guid}/supplies/use")]
    public async Task<IActionResult> UseSupply(Guid id,UseFarmSupplyRequest request,CancellationToken ct)
    {
        if(request.Quantity<=0)return BadRequest("Số lượng sử dụng phải lớn hơn 0.");
        var supply=await db.Supplies.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==request.SupplyId,ct);if(supply is null)return BadRequest("Vật tư không tồn tại.");
        var entries=await db.FarmSupplyEntries.Where(x=>x.FarmId==id&&x.SupplyId==request.SupplyId&&x.SupplyPriceId==request.SupplyPriceId&&x.Quantity>x.UsedQuantity).OrderBy(x=>x.ReceivedAt).ThenBy(x=>x.Id).ToListAsync(ct);
        if(entries.Sum(x=>x.Quantity-x.UsedQuantity)<request.Quantity)return BadRequest("Số lượng sử dụng vượt quá tồn kho của trại.");
        await using var transaction=await db.Database.BeginTransactionAsync(ct);
        var remaining=request.Quantity;decimal totalCost=0;
        foreach(var entry in entries){var take=Math.Min(remaining,entry.Quantity-entry.UsedQuantity);entry.UsedQuantity+=take;totalCost+=take*entry.UnitPrice;remaining-=take;if(remaining==0)break;}
        totalCost=Math.Round(totalCost,2,MidpointRounding.AwayFromZero);
        var usedAt=request.UsedAt==default?DateTime.Today:request.UsedAt.Date;
        var selectedPrice=entries.First().UnitPrice;
        var detail=$"Sử dụng {request.Quantity:N2} {supply.Unit} {supply.Name}, mức giá {selectedPrice:N2} đ/{supply.Unit}. Chi phí vật tư: {totalCost:N2} đ."+(string.IsNullOrWhiteSpace(request.Notes)?string.Empty:$" {request.Notes.Trim()}");
        db.WorkNotes.Add(new FarmWorking.Domain.Entities.WorkNote{FarmId=id,WorkDate=usedAt,Title=$"Sử dụng vật tư: {supply.Name}",Content=detail,Worker=string.IsNullOrWhiteSpace(request.Worker)?"Không ghi nhận":request.Worker.Trim()});
        db.FarmTransactions.Add(new FarmWorking.Domain.Entities.FarmTransaction{FarmId=id,Type=FarmWorking.Domain.Enums.TransactionType.Expense,Amount=totalCost,TransactionDate=usedAt,Tag="Vật tư",Description=$"{supply.Name}: {request.Quantity:N2} {supply.Unit} × {selectedPrice:N2} đ"});
        await db.SaveChangesAsync(ct);await transaction.CommitAsync(ct);return Ok(new{cost=totalCost});
    }

    [HttpDelete("{id:guid}/supplies/{entryId:guid}")]
    public async Task<IActionResult> DeleteSupplyEntry(Guid id,Guid entryId,CancellationToken ct){var x=await db.FarmSupplyEntries.FirstOrDefaultAsync(e=>e.Id==entryId&&e.FarmId==id,ct);if(x is null)return NotFound();if(x.UsedQuantity>0)return BadRequest("Lô vật tư đã được sử dụng nên không thể xóa.");db.FarmSupplyEntries.Remove(x);await db.SaveChangesAsync(ct);return NoContent();}

    [HttpGet("{id:guid}/payroll")]
    public async Task<ActionResult<IEnumerable<WorkerPayroll>>> Payroll(Guid id,CancellationToken ct)
    {
        var rows=await db.WorkerWorkDays.AsNoTracking().Where(x=>x.FarmId==id).OrderByDescending(x=>x.WorkDate).ToListAsync(ct);
        return Ok(rows.GroupBy(x=>x.WorkerId).Select(g=>new WorkerPayroll{WorkerId=g.Key,UnpaidDays=g.Where(x=>!x.SettledAt.HasValue).Sum(x=>x.WorkFraction),UnpaidAmount=g.Where(x=>!x.SettledAt.HasValue).Sum(x=>x.Wage),WorkDays=g.Select(x=>new FarmWorking.Shared.WorkerWorkDay{Id=x.Id,WorkerId=x.WorkerId,WorkTypeId=x.WorkTypeId,WorkTypeName=x.WorkTypeName,WorkDate=x.WorkDate,WorkFraction=x.WorkFraction,Wage=x.Wage,SettledAt=x.SettledAt}).ToList()}));
    }

    [HttpPost("{id:guid}/workers/{workerId:guid}/work-days")]
    public async Task<IActionResult> AddWorkDay(Guid id,Guid workerId,AddWorkerWorkDayRequest request,CancellationToken ct)
    {
        var worker=await db.Workers.FirstOrDefaultAsync(x=>x.Id==workerId&&x.FarmId==id,ct);if(worker is null)return BadRequest("Nhân công không thuộc nông trại này.");if(!worker.IsActive)return BadRequest("Nhân công đã nghỉ nên không thể tính công mới.");
        var workType=await db.WorkTypes.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==request.WorkTypeId&&x.IsActive,ct);if(workType is null)return BadRequest("Công việc không tồn tại hoặc đã ngừng sử dụng.");if(request.WorkFraction<=0||request.WorkFraction>1)return BadRequest("Phần ngày công phải lớn hơn 0 và không vượt quá 1.");
        var date=(request.WorkDate==default?DateTime.Today:request.WorkDate).Date;var assigned=await db.WorkerWorkDays.Where(x=>x.WorkerId==workerId&&x.WorkDate==date).SumAsync(x=>(decimal?)x.WorkFraction,ct)??0;if(assigned+request.WorkFraction>1)return BadRequest($"Tổng công ngày này đã là {assigned:N2}; không thể vượt quá 1 ngày.");
        var wage=Math.Round(workType.DailyRate*request.WorkFraction,2,MidpointRounding.AwayFromZero);db.WorkerWorkDays.Add(new FarmWorking.Domain.Entities.WorkerWorkDay{FarmId=id,WorkerId=workerId,WorkTypeId=workType.Id,WorkTypeName=workType.Name,WorkDate=date,WorkFraction=request.WorkFraction,Wage=wage});await db.SaveChangesAsync(ct);return Ok();
    }

    [HttpPost("{id:guid}/workers/{workerId:guid}/settle-payroll")]
    public async Task<IActionResult> SettlePayroll(Guid id,Guid workerId,SettleWorkerPayrollRequest request,CancellationToken ct)
    {
        var worker=await db.Workers.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==workerId,ct);if(worker is null)return NotFound();var days=await db.WorkerWorkDays.Where(x=>x.FarmId==id&&x.WorkerId==workerId&&!x.SettledAt.HasValue).ToListAsync(ct);if(days.Count==0)return BadRequest("Nhân công không có ngày công chưa thanh toán.");
        var settledAt=DateTime.UtcNow;var settlementDate=(request.SettlementDate==default?DateTime.Today:request.SettlementDate).Date;var total=days.Sum(x=>x.Wage);foreach(var day in days)day.SettledAt=settledAt;
        var totalDays=days.Sum(x=>x.WorkFraction);db.FarmTransactions.Add(new FarmWorking.Domain.Entities.FarmTransaction{FarmId=id,Type=FarmWorking.Domain.Enums.TransactionType.Expense,Amount=total,TransactionDate=settlementDate,Tag="Nhân công",Description=$"Tất toán {totalDays:N2} ngày công cho {worker.FullName}"});await db.SaveChangesAsync(ct);return Ok(new{days=totalDays,amount=total});
    }
}
