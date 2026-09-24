namespace FarmWorking.Shared;

public class FarmProcessRun
{
    public Guid Id { get; set; }
    public Guid FarmId { get; set; }
    public Guid WorkProcessId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string SeasonName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime? CompletedAt { get; set; }
    public List<FarmProcessRunStep> Steps { get; set; } = [];
}

public class FarmProcessRunStep
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DayOffset { get; set; }
    public bool IsFarmExtra { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class StartFarmProcessRunRequest
{
    public string SeasonName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Today;
}
