using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmWorking.Infrastructure.Persistence;

public class FarmDbContext(DbContextOptions<FarmDbContext> options) : DbContext(options)
{
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<WorkNote> WorkNotes => Set<WorkNote>();
    public DbSet<FarmTransaction> FarmTransactions => Set<FarmTransaction>();
    public DbSet<FinanceTag> FinanceTags => Set<FinanceTag>();
    public DbSet<WorkProcess> WorkProcesses => Set<WorkProcess>();
    public DbSet<ProcessStep> ProcessSteps => Set<ProcessStep>();
    public DbSet<FarmProcessExtraStep> FarmProcessExtraSteps => Set<FarmProcessExtraStep>();
    public DbSet<FarmProcessRun> FarmProcessRuns => Set<FarmProcessRun>();
    public DbSet<FarmProcessRunStep> FarmProcessRunSteps => Set<FarmProcessRunStep>();
    public DbSet<KnowledgeItem> KnowledgeItems => Set<KnowledgeItem>();
    public DbSet<Supply> Supplies => Set<Supply>();
    public DbSet<SupplyPrice> SupplyPrices => Set<SupplyPrice>();
    public DbSet<FarmSupplyEntry> FarmSupplyEntries => Set<FarmSupplyEntry>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<WorkerWorkDay> WorkerWorkDays => Set<WorkerWorkDay>();
    public DbSet<WorkType> WorkTypes => Set<WorkType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmDbContext).Assembly);
}
