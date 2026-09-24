using FarmWorking.Application.Abstractions;
using FarmWorking.Domain.Entities;

namespace FarmWorking.Application.Services;

public class ProcessService(IProcessRepository repository, IFarmRepository farms) : IProcessService
{
    public IReadOnlyList<WorkProcess> GetAll() => repository.GetAll();
    public WorkProcess? GetById(Guid id) => repository.Find(id);
    public async Task<WorkProcess> CreateAsync(WorkProcess item, CancellationToken ct = default) { item.Id=Guid.NewGuid(); Normalize(item); repository.Add(item); await repository.SaveChangesAsync(ct); return item; }
    public async Task<WorkProcess?> UpdateAsync(Guid id, WorkProcess item, CancellationToken ct = default)
    {
        var current=repository.Find(id); if(current is null)return null; if(current.IsDeleted)throw new ArgumentException("Quy trình đã xóa, không thể cập nhật.");
        current.Name=item.Name; current.CropType=item.CropType; current.Description=item.Description;
        current.Steps.Clear();
        await repository.SaveChangesAsync(ct);
        var order=1;
        foreach(var step in item.Steps)
        {
            var replacement = new ProcessStep { Id=Guid.NewGuid(), WorkProcessId=current.Id, Order=order++, Title=step.Title, Description=step.Description, DayOffset=step.DayOffset };
            repository.AddStep(replacement);
            current.Steps.Add(replacement);
        }
        await repository.SaveChangesAsync(ct); return current;
    }
    public async Task<bool> DeleteAsync(Guid id,CancellationToken ct=default){var item=repository.Find(id);if(item is null)return false;if(!item.IsDeleted){item.IsDeleted=true;item.DeletedAt=DateTime.UtcNow;await repository.SaveChangesAsync(ct);}return true;}
    public async Task<WorkProcess?> AssignFarmsAsync(Guid id, IReadOnlyCollection<Guid> farmIds, CancellationToken ct=default)
    {
        var item=repository.Find(id); if(item is null)return null; if(item.IsDeleted)throw new ArgumentException("Quy trình đã xóa, không thể gán nông trại.");
        var selectedFarms=new List<Farm>();
        foreach(var farmId in farmIds.Distinct())
        {
            var farm=farms.Find(farmId);
            if(farm is null)throw new ArgumentException($"Nông trại {farmId} không tồn tại.");
            selectedFarms.Add(farm);
        }
        item.Farms.Clear();
        foreach(var farm in selectedFarms)item.Farms.Add(farm);
        await repository.SaveChangesAsync(ct);
        return item;
    }
    private static void Normalize(WorkProcess item){var index=1;foreach(var step in item.Steps){step.Id=Guid.NewGuid();step.WorkProcessId=item.Id;step.Order=index++;}}
}
