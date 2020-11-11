using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Entities.Models;
using FileHelpers;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> Import([FromForm] IFormFile file)
    {
      if(file == null || file.Length < 0)
        return BadRequest("fileIsEmpty");
      var engine = new FileHelperEngine<Transaction>();
      var stream = new StreamReader(file.OpenReadStream());
      var transactions = engine.ReadStream(stream);
      foreach(var transaction in transactions)
      {
        var command = new CreateTransactionCommand()
        {
          Type = transaction.Type,
          ClientName = transaction.ClientName,
          Amount = transaction.Amount,
          Status = transaction.Status
        };
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
        SortTypeBy = query.SortTypeBy,
        SortStatusBy = query.SortStatusBy
      });

      var engine = new FileHelperEngine<Transaction>(){HeaderText = @"TransactionId,Status,Type,ClientName,Amount" };
      var transactionsString = engine.WriteString(transactions);
      return Ok(transactionsString);
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
