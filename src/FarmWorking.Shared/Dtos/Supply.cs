namespace FarmWorking.Shared;

public class Supply
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public SupplyType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "kg";
    public decimal MinimumStock { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Usage { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
