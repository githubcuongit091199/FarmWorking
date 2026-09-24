using FarmWorking.Domain.Entities;
namespace FarmWorking.Application.Models;
public record FarmDetailsResult(Farm Farm, IReadOnlyList<WorkNote> Notes, IReadOnlyList<FarmTransaction> Transactions, IReadOnlyList<WorkProcess> Processes, IReadOnlyList<FarmProcessExtraStep> ExtraSteps, IReadOnlyList<FarmProcessRun> ProcessRuns);
