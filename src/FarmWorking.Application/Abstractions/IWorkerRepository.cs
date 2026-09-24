using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Abstractions;

public interface IWorkerRepository
{
    IReadOnlyList<Worker> GetAll();
    Worker? Find(Guid id);
    void Add(Worker worker);
    void Remove(Worker worker);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
