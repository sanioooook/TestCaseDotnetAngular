using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Attributes.Validators;
using Entities.Enums;
using Entities.Models;

namespace WebAPI.Features.TransactionFeatures.Commands
{
  public class UpdateTransactionCommand : IRequest<Transaction>
  {
    [Required]
    public int Id { get; set; }

    [Required, ValidEnum]
    public EnumStatusTransaction Status { get; set; }

    [Required, ValidEnum]
    public EnumTypeTransaction Type { get; set; }

    [Required]
    public string ClientName { get; set; }

    [Required, Range(0d, double.MaxValue)]
    public double Amount { get; set; }

  }
  public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Transaction>
  {
    private readonly RepositoryContext _context;

    public UpdateTransactionCommandHandler(RepositoryContext context)
    {
      _context = context;
    }

    public async Task<Transaction> Handle(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
      var transaction = await _context.Transactions
        .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
      if(transaction == null)
      {
        return null;
      }
      transaction.Status = command.Status;
      transaction.Amount = command.Amount;
      transaction.ClientName = command.ClientName;
      transaction.Type = command.Type;
      await _context.SaveChangesAsync(cancellationToken);
      return transaction;
    }
  }
}