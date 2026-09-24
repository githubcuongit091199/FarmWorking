namespace FarmWorking.Domain.Entities;

public class Farm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal Area { get; set; }
    public string AreaUnit { get; set; } = "ha";
    public string Crop { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WorkNote> WorkNotes { get; set; } = new List<WorkNote>();
    public ICollection<FarmTransaction> Transactions { get; set; } = new List<FarmTransaction>();
    public ICollection<WorkProcess> WorkProcesses { get; set; } = new List<WorkProcess>();
}
