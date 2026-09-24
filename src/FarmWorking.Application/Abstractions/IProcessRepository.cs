using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Abstractions;
public interface IProcessRepository { IReadOnlyList<WorkProcess> GetAll(); WorkProcess? Find(Guid id); void Add(WorkProcess process); void AddStep(ProcessStep step); void Remove(WorkProcess process); Task SaveChangesAsync(CancellationToken cancellationToken=default); }
