using FarmWorking.Application.Abstractions;
using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Services;

public class TransactionService(IFarmRepository farms, ITransactionRepository repository) : ITransactionService
{
    public IReadOnlyList<FarmTransaction> GetAll() => repository.GetAll();
    public FarmTransaction? GetById(Guid id) => repository.Find(id);

    public async Task<FarmTransaction> CreateAsync(FarmTransaction item, CancellationToken ct = default)
    {
        if (!farms.Exists(item.FarmId)) throw new ArgumentException("Trang trại không tồn tại.");
        if (item.Amount <= 0) throw new ArgumentException("Số tiền phải lớn hơn 0.");

        item.Tag = item.Tag.Trim();
        if (string.IsNullOrWhiteSpace(item.Tag)) throw new ArgumentException("Tag không được để trống.");

        item.Id = Guid.NewGuid();
        if (!repository.TagExists(item.Tag, item.Type))
            repository.AddTag(new FinanceTag { Name = item.Tag, Type = item.Type });

        repository.Add(item);
        await repository.SaveChangesAsync(ct);
        return item;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = repository.Find(id);
        if (item is null) return false;
        repository.Remove(item);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<FarmTransaction?> UpdateAsync(Guid id, FarmTransaction item, CancellationToken ct = default)
    {
        var current = repository.Find(id);
        if (current is null) return null;
        Validate(item);
        item.Tag = item.Tag.Trim();
        if (!repository.TagExists(item.Tag, item.Type))
            repository.AddTag(new FinanceTag { Name = item.Tag, Type = item.Type });
        current.FarmId = item.FarmId;
        current.Type = item.Type;
        current.Amount = item.Amount;
        current.TransactionDate = item.TransactionDate;
        current.Tag = item.Tag;
        current.Description = item.Description;
        await repository.SaveChangesAsync(ct);
        return current;
    }

    private void Validate(FarmTransaction item)
    {
        if (!farms.Exists(item.FarmId)) throw new ArgumentException("Trang trại không tồn tại.");
        if (item.Amount <= 0) throw new ArgumentException("Số tiền phải lớn hơn 0.");
        if (string.IsNullOrWhiteSpace(item.Tag)) throw new ArgumentException("Tag không được để trống.");
    }
}
