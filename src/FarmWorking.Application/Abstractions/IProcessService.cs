using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Abstractions;
public interface IProcessService { IReadOnlyList<WorkProcess> GetAll(); WorkProcess? GetById(Guid id); Task<WorkProcess> CreateAsync(WorkProcess item, CancellationToken ct = default); Task<WorkProcess?> UpdateAsync(Guid id, WorkProcess item, CancellationToken ct = default); Task<WorkProcess?> AssignFarmsAsync(Guid id, IReadOnlyCollection<Guid> farmIds, CancellationToken ct = default); Task<bool> DeleteAsync(Guid id, CancellationToken ct = default); }
