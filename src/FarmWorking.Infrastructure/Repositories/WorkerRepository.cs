using FarmWorking.Application.Abstractions;
using FarmWorking.Domain.Entities;
using FarmWorking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FarmWorking.Infrastructure.Repositories;

public class WorkerRepository(FarmDbContext db) : IWorkerRepository
{
    public IReadOnlyList<Worker> GetAll() => db.Workers.AsNoTracking().OrderBy(x => x.FullName).ToList();
    public Worker? Find(Guid id) => db.Workers.Find(id);
    public void Add(Worker worker) => db.Workers.Add(worker);
    public void Remove(Worker worker) => db.Workers.Remove(worker);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => db.SaveChangesAsync(cancellationToken);
}
