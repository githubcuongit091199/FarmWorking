namespace FarmWorking.Domain.Entities;

public class FarmProcessRunStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmProcessRunId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DayOffset { get; set; }
    public bool IsFarmExtra { get; set; }
    public DateTime? CompletedAt { get; set; }
    public FarmProcessRun Run { get; set; } = null!;
}
