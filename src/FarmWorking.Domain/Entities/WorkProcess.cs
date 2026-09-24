namespace FarmWorking.Domain.Entities;

public class WorkProcess
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string CropType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<Farm> Farms { get; set; } = new List<Farm>();
    public ICollection<ProcessStep> Steps { get; set; } = new List<ProcessStep>();
}
