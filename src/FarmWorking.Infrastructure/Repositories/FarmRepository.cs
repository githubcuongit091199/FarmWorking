using FarmWorking.Application.Abstractions; using FarmWorking.Application.Models; using FarmWorking.Domain.Entities; using FarmWorking.Infrastructure.Persistence; using Microsoft.EntityFrameworkCore;
namespace FarmWorking.Infrastructure.Repositories;
public class FarmRepository(FarmDbContext db):IFarmRepository
{
    public IReadOnlyList<Farm> GetAll()=>db.Farms.AsNoTracking().OrderBy(x=>x.Name).ToList();
    public int Count()=>db.Farms.Count();
    public decimal TotalArea()=>db.Farms.Sum(x=>(decimal?)(
        x.AreaUnit=="m\u00b2"||x.AreaUnit=="m2" ? x.Area/10_000m :
        x.AreaUnit=="s\u00e0o" ? x.Area/10m :
        x.Area))??0;
    public FarmDetailsResult? GetDetails(Guid id){var farm=db.Farms.AsNoTracking().Include(x=>x.WorkNotes).Include(x=>x.Transactions).Include(x=>x.WorkProcesses).ThenInclude(x=>x.Steps).FirstOrDefault(x=>x.Id==id);return farm is null?null:new(farm,farm.WorkNotes.OrderByDescending(x=>x.WorkDate).ToList(),farm.Transactions.OrderByDescending(x=>x.TransactionDate).ToList(),farm.WorkProcesses.ToList(),db.FarmProcessExtraSteps.AsNoTracking().Where(x=>x.FarmId==id).OrderBy(x=>x.Order).ToList(),db.FarmProcessRuns.AsNoTracking().Where(x=>x.FarmId==id).Include(x=>x.Steps).OrderByDescending(x=>x.StartDate).ToList());}
    public bool Exists(Guid id)=>db.Farms.Any(x=>x.Id==id); public Farm? Find(Guid id)=>db.Farms.Find(id); public void Add(Farm farm)=>db.Farms.Add(farm); public void Remove(Farm farm)=>db.Farms.Remove(farm); public void AddNote(WorkNote note)=>db.WorkNotes.Add(note); public WorkNote? FindNote(Guid farmId,Guid noteId)=>db.WorkNotes.FirstOrDefault(x=>x.FarmId==farmId&&x.Id==noteId); public void RemoveNote(WorkNote note)=>db.WorkNotes.Remove(note); public Task SaveChangesAsync(CancellationToken ct=default)=>db.SaveChangesAsync(ct);
    public bool HasProcess(Guid farmId,Guid processId)=>db.Farms.Any(x=>x.Id==farmId&&x.WorkProcesses.Any(p=>p.Id==processId));
    public IReadOnlyList<FarmProcessExtraStep> GetExtraSteps(Guid farmId,Guid processId)=>db.FarmProcessExtraSteps.Where(x=>x.FarmId==farmId&&x.WorkProcessId==processId).OrderBy(x=>x.Order).ToList();
    public void AddExtraStep(FarmProcessExtraStep step)=>db.FarmProcessExtraSteps.Add(step);
    public FarmProcessExtraStep? FindExtraStep(Guid farmId,Guid processId,Guid stepId)=>db.FarmProcessExtraSteps.FirstOrDefault(x=>x.Id==stepId&&x.FarmId==farmId&&x.WorkProcessId==processId);
    public void RemoveExtraStep(FarmProcessExtraStep step)=>db.FarmProcessExtraSteps.Remove(step);
    public WorkProcess? FindAssignedProcess(Guid farmId,Guid processId)=>db.WorkProcesses.Include(x=>x.Steps).FirstOrDefault(x=>x.Id==processId&&x.Farms.Any(f=>f.Id==farmId));
    public void AddProcessRun(FarmProcessRun run)=>db.FarmProcessRuns.Add(run);
    public FarmProcessRun? FindProcessRun(Guid farmId,Guid runId)=>db.FarmProcessRuns.Include(x=>x.Steps).FirstOrDefault(x=>x.Id==runId&&x.FarmId==farmId);
    public void RemoveProcessRun(FarmProcessRun run)=>db.FarmProcessRuns.Remove(run);
}
