namespace FarmWorking.Shared;

public class FinanceTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
}
