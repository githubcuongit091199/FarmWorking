using FarmWorking.Application.Models;
using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Abstractions;

public interface IFarmRepository
{
    IReadOnlyList<Farm> GetAll();
    int Count();
    decimal TotalArea();
    FarmDetailsResult? GetDetails(Guid id);
    bool Exists(Guid id);
    Farm? Find(Guid id);
    void Add(Farm farm);
    void Remove(Farm farm);
    void AddNote(WorkNote note);
    WorkNote? FindNote(Guid farmId, Guid noteId);
    void RemoveNote(WorkNote note);
    bool HasProcess(Guid farmId, Guid processId);
    IReadOnlyList<FarmProcessExtraStep> GetExtraSteps(Guid farmId, Guid processId);
    void AddExtraStep(FarmProcessExtraStep step);
    FarmProcessExtraStep? FindExtraStep(Guid farmId, Guid processId, Guid stepId);
    void RemoveExtraStep(FarmProcessExtraStep step);
    WorkProcess? FindAssignedProcess(Guid farmId, Guid processId);
    void AddProcessRun(FarmProcessRun run);
    FarmProcessRun? FindProcessRun(Guid farmId, Guid runId);
    void RemoveProcessRun(FarmProcessRun run);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
