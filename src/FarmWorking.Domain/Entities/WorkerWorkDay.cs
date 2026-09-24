namespace FarmWorking.Domain.Entities;

public class WorkerWorkDay
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public Guid WorkerId { get; set; }
    public Guid WorkTypeId { get; set; }
    public string WorkTypeName { get; set; } = string.Empty;
    public DateTime WorkDate { get; set; } = DateTime.Today;
    public decimal WorkFraction { get; set; } = 1;
    public decimal Wage { get; set; }
    public DateTime? SettledAt { get; set; }
    public Farm Farm { get; set; } = null!;
    public Worker Worker { get; set; } = null!;
    public WorkType WorkType { get; set; } = null!;
}
