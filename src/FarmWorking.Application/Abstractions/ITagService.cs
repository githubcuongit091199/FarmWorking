using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Abstractions;
public interface ITagService
{
    IReadOnlyList<FinanceTag> GetAll();
    FinanceTag? GetById(Guid id);
    Task<FinanceTag> CreateAsync(FinanceTag item, CancellationToken ct = default);
    Task<FinanceTag?> UpdateAsync(Guid id, FinanceTag item, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
