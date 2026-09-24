using FarmWorking.Domain.Entities;
using FarmWorking.Domain.Enums;
namespace FarmWorking.Application.Abstractions;
public interface ISupplyService { IReadOnlyList<Supply> GetAll(SupplyType? type = null); Supply? GetById(Guid id); Task<Supply> CreateAsync(Supply item, CancellationToken ct = default); Task<Supply?> UpdateAsync(Guid id, Supply item, CancellationToken ct = default); Task<bool> DeleteAsync(Guid id, CancellationToken ct = default); }
