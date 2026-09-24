using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FarmWorking.Api.Controllers;

[ApiController, Route("api/workers")]
public class WorkersController(IWorkerService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Worker>> GetAll() => Ok(service.GetAll().Select(x => x.ToContract()));

    [HttpGet("{id:guid}")]
    public ActionResult<Worker> Get(Guid id)
    {
        var worker = service.GetById(id);
        return worker is null ? NotFound() : Ok(worker.ToContract());
    }

    [HttpPost]
    public async Task<ActionResult<Worker>> Create(Worker worker, CancellationToken ct)
    {
        try
        {
            var result = (await service.CreateAsync(worker.ToDomain(), ct)).ToContract();
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Worker>> Update(Guid id, Worker worker, CancellationToken ct)
    {
        try
        {
            var result = await service.UpdateAsync(id, worker.ToDomain(), ct);
            return result is null ? NotFound() : Ok(result.ToContract());
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct) =>
        await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
