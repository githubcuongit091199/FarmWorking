namespace FarmWorking.Domain.Entities;

public class FarmProcessExtraStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public Guid WorkProcessId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DayOffset { get; set; }

    public Farm Farm { get; set; } = null!;
    public WorkProcess WorkProcess { get; set; } = null!;
}
