namespace FarmWorking.Shared;

public class SupplyPrice
{
    public Guid Id { get; set; }
    public Guid SupplyId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class AddSupplyPriceRequest { public decimal Price { get; set; } }
public class ProvisionFarmSupplyRequest { public Guid SupplyId { get; set; } public Guid SupplyPriceId { get; set; } public decimal Quantity { get; set; } public DateTime ReceivedAt { get; set; }=DateTime.Today; }
public class UseFarmSupplyRequest
{
    public Guid SupplyId { get; set; }
    public Guid SupplyPriceId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime UsedAt { get; set; } = DateTime.Today;
    public string Worker { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
public class FarmSupplyEntry
{
    public Guid Id { get; set; }
    public Guid FarmId { get; set; }
    public Guid SupplyId { get; set; }
    public Guid SupplyPriceId { get; set; }
    public string SupplyName { get; set; }=string.Empty;
    public SupplyType SupplyType { get; set; }
    public string Unit { get; set; }=string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal UsedQuantity { get; set; }
    public DateTime ReceivedAt { get; set; }
    public decimal RemainingQuantity=>Quantity-UsedQuantity;
}
