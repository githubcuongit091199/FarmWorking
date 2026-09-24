using FarmWorking.Application.Abstractions;
using FarmWorking.Application.Models;
using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Services;

public class FarmService(IFarmRepository repository) : IFarmService
{
    public IReadOnlyList<Farm> GetAll() => repository.GetAll();
    public FarmDetailsResult? GetById(Guid id) => repository.GetDetails(id);

    public async Task<Farm> CreateAsync(Farm farm, CancellationToken ct = default)
    {
        farm.Id = Guid.NewGuid();
        farm.CreatedAt = DateTime.UtcNow;
        repository.Add(farm);
        await repository.SaveChangesAsync(ct);
        return farm;
    }

    public async Task<Farm?> UpdateAsync(Guid id, Farm farm, CancellationToken ct = default)
    {
        var current = repository.Find(id);
        if (current is null) return null;

        current.Name = farm.Name;
        current.Location = farm.Location;
        current.Area = farm.Area;
        current.AreaUnit = farm.AreaUnit;
        current.Latitude = farm.Latitude;
        current.Longitude = farm.Longitude;
        current.Crop = farm.Crop;
        current.Description = farm.Description;
        await repository.SaveChangesAsync(ct);
        return current;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var farm = repository.Find(id);
        if (farm is null) return false;
        repository.Remove(farm);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<WorkNote?> AddNoteAsync(Guid farmId, WorkNote note, CancellationToken ct = default)
    {
        if (!repository.Exists(farmId)) return null;
        note.Id = Guid.NewGuid();
        note.FarmId = farmId;
        repository.AddNote(note);
        await repository.SaveChangesAsync(ct);
        return note;
    }

    public WorkNote? GetNote(Guid farmId, Guid noteId) => repository.FindNote(farmId, noteId);

    public async Task<WorkNote?> UpdateNoteAsync(Guid farmId, Guid noteId, WorkNote note, CancellationToken ct = default)
    {
        var current = repository.FindNote(farmId, noteId);
        if (current is null) return null;
        current.WorkDate = note.WorkDate;
        current.Title = note.Title;
        current.Content = note.Content;
        current.Worker = note.Worker;
        await repository.SaveChangesAsync(ct);
        return current;
    }

    public async Task<bool> DeleteNoteAsync(Guid farmId, Guid noteId, CancellationToken ct = default)
    {
        var note = repository.FindNote(farmId, noteId);
        if (note is null) return false;
        repository.RemoveNote(note);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<FarmProcessExtraStep?> AddExtraStepAsync(Guid farmId, Guid processId, FarmProcessExtraStep step, CancellationToken ct = default)
    {
        if (!repository.HasProcess(farmId, processId)) return null;
        if (string.IsNullOrWhiteSpace(step.Title)) throw new ArgumentException("Tên bước bổ sung không được để trống.");
        var existing = repository.GetExtraSteps(farmId, processId);
        step.Id = Guid.NewGuid();
        step.FarmId = farmId;
        step.WorkProcessId = processId;
        step.Order = existing.Count == 0 ? 1 : existing.Max(x => x.Order) + 1;
        repository.AddExtraStep(step);
        await repository.SaveChangesAsync(ct);
        return step;
    }

    public async Task<bool> DeleteExtraStepAsync(Guid farmId, Guid processId, Guid stepId, CancellationToken ct = default)
    {
        var step = repository.FindExtraStep(farmId, processId, stepId);
        if (step is null) return false;
        repository.RemoveExtraStep(step);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<FarmProcessRun?> StartProcessRunAsync(Guid farmId, Guid processId, string seasonName, DateTime startDate, CancellationToken ct = default)
    {
        var process = repository.FindAssignedProcess(farmId, processId);
        if (process is null || process.IsDeleted) return null;
        var extraSteps = repository.GetExtraSteps(farmId, processId);
        var sourceSteps = process.Steps.OrderBy(x=>x.Order).Select(x=>(x.Title,x.Description,x.DayOffset,false))
            .Concat(extraSteps.OrderBy(x=>x.Order).Select(x=>(x.Title,x.Description,x.DayOffset,true))).ToList();
        if (sourceSteps.Count == 0) throw new ArgumentException("Quy trình cần có ít nhất một bước trước khi bắt đầu.");
        var run = new FarmProcessRun { Id=Guid.NewGuid(), FarmId=farmId, WorkProcessId=processId, ProcessName=process.Name, SeasonName=string.IsNullOrWhiteSpace(seasonName)?$"Mùa vụ {startDate:yyyy}":seasonName.Trim(), StartDate=startDate.Date };
        var order=1;
        foreach(var step in sourceSteps) run.Steps.Add(new FarmProcessRunStep { Id=Guid.NewGuid(), Order=order++, Title=step.Title, Description=step.Description, DayOffset=step.DayOffset, IsFarmExtra=step.Item4 });
        repository.AddProcessRun(run);
        await repository.SaveChangesAsync(ct);
        return run;
    }

    public async Task<FarmProcessRun?> SetRunStepCompletedAsync(Guid farmId, Guid runId, Guid stepId, bool completed, CancellationToken ct = default)
    {
        var run=repository.FindProcessRun(farmId,runId); if(run is null)return null;
        var step=run.Steps.FirstOrDefault(x=>x.Id==stepId); if(step is null)return null;
        step.CompletedAt=completed?DateTime.UtcNow:null;
        run.CompletedAt=run.Steps.All(x=>x.CompletedAt.HasValue)?DateTime.UtcNow:null;
        await repository.SaveChangesAsync(ct);
        return run;
    }

    public async Task<bool> DeleteProcessRunAsync(Guid farmId, Guid runId, CancellationToken ct = default)
    {
        var run=repository.FindProcessRun(farmId,runId);if(run is null)return false;
        repository.RemoveProcessRun(run);await repository.SaveChangesAsync(ct);return true;
    }
}
