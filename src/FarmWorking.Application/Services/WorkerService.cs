using FarmWorking.Application.Abstractions;
using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Services;

public class WorkerService(IWorkerRepository repository) : IWorkerService
{
    public IReadOnlyList<Worker> GetAll() => repository.GetAll();
    public Worker? GetById(Guid id) => repository.Find(id);

    public async Task<Worker> CreateAsync(Worker worker, CancellationToken ct = default)
    {
        Validate(worker);
        worker.Id = Guid.NewGuid();
        repository.Add(worker);
        await repository.SaveChangesAsync(ct);
        return worker;
    }

    public async Task<Worker?> UpdateAsync(Guid id, Worker worker, CancellationToken ct = default)
    {
        var current = repository.Find(id);
        if (current is null) return null;
        Validate(worker);
        current.FarmId = worker.FarmId;
        current.FullName = worker.FullName.Trim();
        current.Phone = worker.Phone.Trim();
        current.Address = worker.Address.Trim();
        current.Role = worker.Role.Trim();
        current.DailyWage = worker.DailyWage;
        current.StartDate = worker.StartDate;
        current.IsActive = worker.IsActive;
        current.Notes = worker.Notes.Trim();
        await repository.SaveChangesAsync(ct);
        return current;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var worker = repository.Find(id);
        if (worker is null) return false;
        repository.Remove(worker);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    private static void Validate(Worker worker)
    {
        if (string.IsNullOrWhiteSpace(worker.FullName)) throw new ArgumentException("Họ tên nhân công không được để trống.");
        if (worker.DailyWage < 0) throw new ArgumentException("Mức lương ngày không được âm.");
        worker.FullName = worker.FullName.Trim();
    }
}
