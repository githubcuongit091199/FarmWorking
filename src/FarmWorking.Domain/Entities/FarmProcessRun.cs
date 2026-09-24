namespace FarmWorking.Domain.Entities;

public class FarmProcessRun
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public Guid WorkProcessId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string SeasonName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Farm Farm { get; set; } = null!;
    public WorkProcess WorkProcess { get; set; } = null!;
    public ICollection<FarmProcessRunStep> Steps { get; set; } = new List<FarmProcessRunStep>();
}
