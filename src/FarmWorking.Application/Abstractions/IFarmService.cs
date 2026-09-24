using FarmWorking.Application.Models;
using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Abstractions;
public interface IFarmService
{
    IReadOnlyList<Farm> GetAll();
    FarmDetailsResult? GetById(Guid id);
    Task<Farm> CreateAsync(Farm farm, CancellationToken ct = default);
    Task<Farm?> UpdateAsync(Guid id, Farm farm, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    WorkNote? GetNote(Guid farmId, Guid noteId);
    Task<WorkNote?> AddNoteAsync(Guid farmId, WorkNote note, CancellationToken ct = default);
    Task<WorkNote?> UpdateNoteAsync(Guid farmId, Guid noteId, WorkNote note, CancellationToken ct = default);
    Task<bool> DeleteNoteAsync(Guid farmId, Guid noteId, CancellationToken ct = default);
    Task<FarmProcessExtraStep?> AddExtraStepAsync(Guid farmId, Guid processId, FarmProcessExtraStep step, CancellationToken ct = default);
    Task<bool> DeleteExtraStepAsync(Guid farmId, Guid processId, Guid stepId, CancellationToken ct = default);
    Task<FarmProcessRun?> StartProcessRunAsync(Guid farmId, Guid processId, string seasonName, DateTime startDate, CancellationToken ct = default);
    Task<FarmProcessRun?> SetRunStepCompletedAsync(Guid farmId, Guid runId, Guid stepId, bool completed, CancellationToken ct = default);
    Task<bool> DeleteProcessRunAsync(Guid farmId, Guid runId, CancellationToken ct = default);
}
