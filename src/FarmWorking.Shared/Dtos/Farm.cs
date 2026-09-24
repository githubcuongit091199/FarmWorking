namespace FarmWorking.Shared;

/// <summary>Contract trao đổi thông tin nông trại giữa API và UI.</summary>
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
}
