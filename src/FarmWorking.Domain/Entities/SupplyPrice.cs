namespace FarmWorking.Domain.Entities;

public class SupplyPrice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SupplyId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Supply Supply { get; set; } = null!;
}
