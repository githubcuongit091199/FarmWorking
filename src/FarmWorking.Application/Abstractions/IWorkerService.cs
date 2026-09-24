using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Abstractions;

public interface IWorkerService
{
    IReadOnlyList<Worker> GetAll();
    Worker? GetById(Guid id);
    Task<Worker> CreateAsync(Worker worker, CancellationToken ct = default);
    Task<Worker?> UpdateAsync(Guid id, Worker worker, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
