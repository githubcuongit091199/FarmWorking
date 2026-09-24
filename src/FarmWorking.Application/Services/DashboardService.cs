using FarmWorking.Application.Abstractions;
using FarmWorking.Application.Models;
using FarmWorking.Domain.Enums;

namespace FarmWorking.Application.Services;

public class DashboardService(
    IFarmRepository farms,
    ITransactionRepository transactions,
    ISupplyRepository supplies) : IDashboardService
{
    public DashboardResult GetSummary() => new(
        farms.Count(),
        farms.TotalArea(),
        transactions.Sum(TransactionType.Income),
        transactions.Sum(TransactionType.Expense),
        supplies.CountLowStock(),
        transactions.GetRecent(6));
}
