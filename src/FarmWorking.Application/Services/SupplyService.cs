using FarmWorking.Application.Abstractions; using FarmWorking.Domain.Entities; using FarmWorking.Domain.Enums;
namespace FarmWorking.Application.Services;
public class SupplyService(ISupplyRepository repository):ISupplyService
{
    public IReadOnlyList<Supply> GetAll(SupplyType? type=null)=>repository.GetAll(type);
    public Supply? GetById(Guid id)=>repository.Find(id);
    public async Task<Supply> CreateAsync(Supply item,CancellationToken ct=default){item.Id=Guid.NewGuid();repository.Add(item);await repository.SaveChangesAsync(ct);return item;}
    public async Task<Supply?> UpdateAsync(Guid id,Supply item,CancellationToken ct=default){var current=repository.Find(id);if(current is null)return null;current.Type=item.Type;current.Name=item.Name;current.Brand=item.Brand;current.ImageUrl=item.ImageUrl;current.Quantity=item.Quantity;current.Unit=item.Unit;current.MinimumStock=item.MinimumStock;current.ExpiryDate=item.ExpiryDate;current.Usage=item.Usage;current.Notes=item.Notes;await repository.SaveChangesAsync(ct);return current;}
    public async Task<bool> DeleteAsync(Guid id,CancellationToken ct=default){var item=repository.Find(id);if(item is null)return false;repository.Remove(item);await repository.SaveChangesAsync(ct);return true;}
}
