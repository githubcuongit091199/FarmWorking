using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/processes")]
public class ProcessesController(IProcessService service) : ControllerBase
{
    [HttpGet] public ActionResult<IEnumerable<WorkProcess>> GetAll() => Ok(service.GetAll().Select(x => x.ToContract()));
    [HttpGet("{id:guid}")] public ActionResult<WorkProcess> Get(Guid id) { var result = service.GetById(id); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpPost] public async Task<ActionResult<WorkProcess>> Create(WorkProcess item, CancellationToken ct) { try { var created=await service.CreateAsync(item.ToDomain(),ct);var result=await service.AssignFarmsAsync(created.Id,item.FarmIds,ct)??created;return CreatedAtAction(nameof(Get),new{id=result.Id},result.ToContract()); } catch(ArgumentException ex){return BadRequest(ex.Message);} }
    [HttpPut("{id:guid}")] public async Task<ActionResult<WorkProcess>> Update(Guid id, WorkProcess item, CancellationToken ct) { try { var updated=await service.UpdateAsync(id,item.ToDomain(),ct);if(updated is null)return NotFound();var result=await service.AssignFarmsAsync(id,item.FarmIds,ct)??updated;return Ok(result.ToContract()); } catch(ArgumentException ex){return BadRequest(ex.Message);} }
    [HttpPut("{id:guid}/farms")] public async Task<ActionResult<WorkProcess>> AssignFarms(Guid id, AssignProcessRequest request, CancellationToken ct) { try { var result = await service.AssignFarmsAsync(id, request.FarmIds, ct); return result is null ? NotFound() : Ok(result.ToContract()); } catch (ArgumentException ex) { return BadRequest(ex.Message); } }
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id, CancellationToken ct) => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
