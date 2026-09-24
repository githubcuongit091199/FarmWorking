using FarmWorking.Domain.Entities; using FarmWorking.Domain.Enums;
namespace FarmWorking.Application.Abstractions;
public interface ISupplyRepository { IReadOnlyList<Supply> GetAll(SupplyType? type=null); Supply? Find(Guid id); int CountLowStock(); void Add(Supply supply); void Remove(Supply supply); Task SaveChangesAsync(CancellationToken cancellationToken=default); }
