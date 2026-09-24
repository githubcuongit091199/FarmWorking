using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Abstractions;
public interface ITransactionService { IReadOnlyList<FarmTransaction> GetAll(); FarmTransaction? GetById(Guid id); Task<FarmTransaction> CreateAsync(FarmTransaction item, CancellationToken ct = default); Task<FarmTransaction?> UpdateAsync(Guid id, FarmTransaction item, CancellationToken ct = default); Task<bool> DeleteAsync(Guid id, CancellationToken ct = default); }
