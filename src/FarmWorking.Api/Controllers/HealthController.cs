using Microsoft.AspNetCore.Mvc;
namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/health")]
public class HealthController : ControllerBase { [HttpGet] public IActionResult Get() => Ok(new { status = "healthy", time = DateTimeOffset.Now }); }
