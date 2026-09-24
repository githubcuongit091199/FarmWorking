namespace FarmWorking.Shared;

public class FarmProcessExtraStep
{
    public Guid Id { get; set; }
    public Guid FarmId { get; set; }
    public Guid WorkProcessId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DayOffset { get; set; }
}
