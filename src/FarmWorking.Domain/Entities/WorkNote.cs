namespace FarmWorking.Domain.Entities;

public class WorkNote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public DateTime WorkDate { get; set; } = DateTime.Today;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Worker { get; set; } = string.Empty;

    public Farm Farm { get; set; } = null!;
}
