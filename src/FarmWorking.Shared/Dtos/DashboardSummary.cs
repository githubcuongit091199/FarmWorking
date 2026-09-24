namespace FarmWorking.Shared;
public class DashboardSummary { public int FarmCount { get; set; } public decimal TotalArea { get; set; } public decimal Income { get; set; } public decimal Expense { get; set; } public int LowStockCount { get; set; } public List<FarmTransaction> RecentTransactions { get; set; } = []; }
