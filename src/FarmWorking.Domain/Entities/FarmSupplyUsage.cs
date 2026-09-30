namespace FarmWorking.Domain.Entities;

public class FarmSupplyUsage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public Guid SupplyId { get; set; }
    public Guid SupplyPriceId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public DateTime UsedAt { get; set; } = DateTime.Today;
    public string Worker { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Farm Farm { get; set; } = null!;
    public Supply Supply { get; set; } = null!;
    public SupplyPrice SupplyPrice { get; set; } = null!;
}
