namespace FarmWorking.Domain.Entities;

public class ProcessStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkProcessId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DayOffset { get; set; }

    public WorkProcess WorkProcess { get; set; } = null!;
}
