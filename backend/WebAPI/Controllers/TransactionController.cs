using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileHelpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Features.TransactionFeatures.Commands.CreateTransaction;
using WebAPI.Features.TransactionFeatures.Commands.DeleteTransactionById;
using WebAPI.Features.TransactionFeatures.Commands.UpdateTransaction;
using WebAPI.Features.TransactionFeatures.Queries.ExportTransactions;
using WebAPI.Features.TransactionFeatures.Queries.GetAllTransactions;
using WebAPI.Features.TransactionFeatures.Queries.GetTransactionById;

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionController : ControllerBase
  {
    public TransactionController (IMediator mediator)
    {
      Mediator = mediator;
    }

    private IMediator Mediator { get; }

    // POST: api/Transaction
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionCommand command)
    {
      var resultCreate = await Mediator.Send(command);
      return resultCreate != 0 ? (IActionResult)Ok() : BadRequest("This entry already exists in the database");
    }

    // POST: api/Transaction/Import
    [HttpPost("Import")]
    public async Task<IActionResult> Import([FromForm] IFormFile file)
    {
      if(file == null || file.Length <= 0)
        return BadRequest("fileIsEmpty");
      var engine = new FileHelperEngine<CreateTransactionCommand>();
      var stream = new StreamReader(file.OpenReadStream());
      var transactions = engine.ReadStream(stream);
      foreach(var transaction in transactions)
      {
        await Mediator.Send(transaction);
      }
      return Ok();
    }

    // GET: api/Transaction
    [HttpGet,ProducesResponseType(typeof(TransactionForGetAll), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllTransactionsQuery query)
    {
      return Ok(await Mediator.Send(query));
    }

    // GET: api/Transaction/Export
    [HttpGet("Export"),ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFileExport([FromQuery] ExportTransactionsQuery query)
    {
      var transactions = await Mediator.Send(query);

      return File(
        transactions.ToArray(),
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "data.xlsx");
    }

    // GET: api/Transaction/id
    [HttpGet("{id}"),ProducesResponseType(typeof(TransactionForGetById), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
      var transaction = await Mediator.Send(new GetTransactionByIdQuery {Id = id});
      return transaction == null? (IActionResult)NotFound(): Ok(transaction);
    }

    // DELETE: api/Transaction/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var transactionId = await Mediator.Send(new DeleteTransactionByIdCommand {Id = id});
      return transactionId != id ? (IActionResult)NotFound() : Ok();
    }

    // PUT: api/Transaction/id
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTransactionCommand command)
    {
      if(id != command.Id)
      {
        return BadRequest();
      }

      var updatedTransactionId = await Mediator.Send(command);
      return updatedTransactionId != id ? (IActionResult)NotFound() : Ok(updatedTransactionId);
    }
  }
}
