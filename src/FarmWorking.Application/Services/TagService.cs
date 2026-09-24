using FarmWorking.Application.Abstractions;
using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Services;
public class TagService(ITransactionRepository repository) : ITagService
{
    public IReadOnlyList<FinanceTag> GetAll() => repository.GetTags();
    public FinanceTag? GetById(Guid id) => repository.FindTag(id);

    public async Task<FinanceTag> CreateAsync(FinanceTag item, CancellationToken ct = default)
    {
        NormalizeAndValidate(item);
        if (repository.TagExists(item.Name, item.Type)) throw new ArgumentException("Tag đã tồn tại.");
        item.Id = Guid.NewGuid();
        repository.AddTag(item);
        await repository.SaveChangesAsync(ct);
        return item;
    }

    public async Task<FinanceTag?> UpdateAsync(Guid id, FinanceTag item, CancellationToken ct = default)
    {
        var current = repository.FindTag(id);
        if (current is null) return null;
        NormalizeAndValidate(item);
        if ((current.Name != item.Name || current.Type != item.Type) && repository.TagExists(item.Name, item.Type))
            throw new ArgumentException("Tag đã tồn tại.");
        current.Name = item.Name;
        current.Type = item.Type;
        await repository.SaveChangesAsync(ct);
        return current;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var item = repository.FindTag(id);
        if (item is null) return false;
        repository.RemoveTag(item);
        await repository.SaveChangesAsync(ct);
        return true;
    }

    private static void NormalizeAndValidate(FinanceTag item)
    {
        item.Name = item.Name.Trim();
        if (string.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException("Tên tag không được để trống.");
    }
}
