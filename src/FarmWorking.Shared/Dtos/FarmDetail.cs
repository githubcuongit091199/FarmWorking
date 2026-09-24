namespace FarmWorking.Shared;
public class FarmDetail { public Farm Farm { get; set; } = new(); public List<WorkNote> Notes { get; set; } = []; public List<FarmTransaction> Transactions { get; set; } = []; public List<WorkProcess> Processes { get; set; } = []; public List<FarmProcessExtraStep> ExtraSteps { get; set; } = []; public List<FarmProcessRun> ProcessRuns { get; set; } = []; }
