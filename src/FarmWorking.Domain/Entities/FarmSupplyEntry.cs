namespace FarmWorking.Domain.Entities;

public class FarmSupplyEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public Guid SupplyId { get; set; }
    public Guid SupplyPriceId { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal UsedQuantity { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public Farm Farm { get; set; } = null!;
    public Supply Supply { get; set; } = null!;
    public SupplyPrice SupplyPrice { get; set; } = null!;
}
