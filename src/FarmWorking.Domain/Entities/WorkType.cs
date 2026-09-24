namespace FarmWorking.Domain.Entities;
public class WorkType
{
    public Guid Id { get; set; }=Guid.NewGuid();
    public string Name { get; set; }=string.Empty;
    public decimal DailyRate { get; set; }
    public bool IsActive { get; set; }=true;
}
