using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Classes;
using WebAPI.Features.TransactionFeatures.Commands;
using WebAPI.Features.TransactionFeatures.Queries;

namespace WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TransactionController : ControllerBase
  {
    private IMediator _mediator;

    protected IMediator Mediator
    {
      get { return _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); }
    }

    // POST: api/Transaction
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionCommand command)
    {
      return Ok(await Mediator.Send(command));
    }


    // POST: api/Transaction/Import
    [HttpPost("Import")]
    public async Task<IActionResult> Import(CreateTransactionCommand[] commands)
    {
      foreach(var command in commands)
      {
        await Mediator.Send(command);
      }
      return Ok();
    }

    // GET: api/Transaction
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Pagination<Transaction> query)
    {
      return Ok(await Mediator.Send(new GetAllTransactionsQuery(){Pagination = query}));
    }

    // GET: api/Transaction/Export
    [HttpGet("Export")]
    public async Task<IActionResult> GetFileExport([FromQuery] ExportTransactionsQuery query)
    {
      var transactions = await Mediator.Send(new ExportTransactionsQuery()
      {
        SortTypeTransaction = query.SortTypeTransaction,
        SortStatusTransaction = query.SortStatusTransaction
      });

      var contentFile = new List<byte>();
      contentFile.AddRange(System.Text.Encoding.UTF8.GetBytes("TransactionId,Status,Type,ClientName,Amount\n"));
      foreach(var transaction in transactions)
      {
        contentFile.AddRange(System.Text.Encoding.UTF8.GetBytes(transaction.ToString()));
      }
      return new FileContentResult(contentFile.ToArray(), contentType: "text/csv");
    }

    // GET: api/Transaction/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return Ok(await Mediator.Send(new GetTransactionByIdQuery { Id = id }));
    }

    // DELETE: api/Transaction/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      return Ok(await Mediator.Send(new DeleteTransactionByIdCommand { Id = id }));
    }

    // PUT: api/Transaction/id
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTransactionCommand command)
    {
      if(id != command.Id)
      {
        return BadRequest();
      }
      return Ok(await Mediator.Send(command));
    }
  }
}
