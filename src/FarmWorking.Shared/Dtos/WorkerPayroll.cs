namespace FarmWorking.Shared;

public class AddWorkerWorkDayRequest { public DateTime WorkDate { get; set; } = DateTime.Today; public Guid WorkTypeId { get; set; } public decimal WorkFraction { get; set; }=1; }
public class SettleWorkerPayrollRequest { public DateTime SettlementDate { get; set; } = DateTime.Today; }
public class WorkerWorkDay
{
    public Guid Id { get; set; }
    public Guid WorkerId { get; set; }
    public Guid WorkTypeId { get; set; }
    public string WorkTypeName { get; set; }=string.Empty;
    public DateTime WorkDate { get; set; }
    public decimal WorkFraction { get; set; }
    public decimal Wage { get; set; }
    public DateTime? SettledAt { get; set; }
}
public class WorkerPayroll
{
    public Guid WorkerId { get; set; }
    public decimal UnpaidDays { get; set; }
    public decimal UnpaidAmount { get; set; }
    public List<WorkerWorkDay> WorkDays { get; set; } = [];
}
public class WorkType { public Guid Id { get; set; } public string Name { get; set; }=string.Empty; public decimal DailyRate { get; set; } public bool IsActive { get; set; }=true; }
