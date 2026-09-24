namespace FarmWorking.Shared;

public class WorkProcess
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string CropType { get; set; } = string.Empty;
    public List<Guid> FarmIds { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public List<ProcessStep> Steps { get; set; } = [];
}
