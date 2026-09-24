using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/dashboard")]
public class DashboardController(IDashboardService service) : ControllerBase
{
    [HttpGet] public ActionResult<DashboardSummary> Get() { var x = service.GetSummary(); return Ok(new DashboardSummary { FarmCount = x.FarmCount, TotalArea = x.TotalArea, Income = x.Income, Expense = x.Expense, LowStockCount = x.LowStockCount, RecentTransactions = x.RecentTransactions.Select(y => y.ToContract()).ToList() }); }
}
