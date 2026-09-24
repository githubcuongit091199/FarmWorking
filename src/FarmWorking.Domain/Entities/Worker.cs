namespace FarmWorking.Domain.Entities;

public class Worker
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? FarmId { get; set; }
    public Farm? Farm { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public decimal DailyWage { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public bool IsActive { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
}
