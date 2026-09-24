using FarmWorking.Domain.Entities;
using FarmWorking.Domain.Enums;

namespace FarmWorking.Application.Abstractions;

public interface ITransactionRepository
{
    IReadOnlyList<FarmTransaction> GetAll();
    IReadOnlyList<FarmTransaction> GetRecent(int count);
    IReadOnlyList<FinanceTag> GetTags();
    FinanceTag? FindTag(Guid id);
    FarmTransaction? Find(Guid id);
    bool TagExists(string name, TransactionType type);
    decimal Sum(TransactionType type);
    void Add(FarmTransaction transaction);
    void AddTag(FinanceTag tag);
    void RemoveTag(FinanceTag tag);
    void Remove(FarmTransaction transaction);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
