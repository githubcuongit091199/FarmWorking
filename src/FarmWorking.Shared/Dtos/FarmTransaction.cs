namespace FarmWorking.Shared;

public class FarmTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FarmId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Today;
    public string Tag { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
