using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Models;
public record DashboardResult(int FarmCount, decimal TotalArea, decimal Income, decimal Expense, int LowStockCount, IReadOnlyList<FarmTransaction> RecentTransactions);
