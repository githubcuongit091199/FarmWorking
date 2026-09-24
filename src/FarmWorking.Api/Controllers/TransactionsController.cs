using FarmWorking.Api.Mappings;
using FarmWorking.Application.Abstractions;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FarmWorking.Api.Controllers;
[ApiController, Route("api/transactions")]
public class TransactionsController(ITransactionService service) : ControllerBase
{
    [HttpGet] public ActionResult<IEnumerable<FarmTransaction>> GetAll() => Ok(service.GetAll().Select(x => x.ToContract()));
    [HttpGet("{id:guid}")] public ActionResult<FarmTransaction> Get(Guid id) { var result = service.GetById(id); return result is null ? NotFound() : Ok(result.ToContract()); }
    [HttpPost] public async Task<ActionResult<FarmTransaction>> Create(FarmTransaction item, CancellationToken ct) { try { var result = (await service.CreateAsync(item.ToDomain(), ct)).ToContract(); return CreatedAtAction(nameof(Get), new { id = result.Id }, result); } catch (ArgumentException ex) { return BadRequest(ex.Message); } }
    [HttpPut("{id:guid}")] public async Task<ActionResult<FarmTransaction>> Update(Guid id, FarmTransaction item, CancellationToken ct) { try { var result = await service.UpdateAsync(id, item.ToDomain(), ct); return result is null ? NotFound() : Ok(result.ToContract()); } catch (ArgumentException ex) { return BadRequest(ex.Message); } }
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id, CancellationToken ct) => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
