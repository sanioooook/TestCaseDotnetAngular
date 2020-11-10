using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using MediatR;
using WebAPI.Attributes.Validators;
using Entities.Enums;

namespace WebAPI.Features.TransactionFeatures.Commands
{
  public class CreateTransactionCommand: IRequest<int>
  {
    [Required, ValidEnum]
    public EnumStatusTransaction Status { get; set; }

    [Required, ValidEnum]
    public EnumTypeTransaction Type { get; set; }

    [Required]
    public string ClientName { get; set; }

    [Required, Range(0d, double.MaxValue)]
    public double Amount { get; set; }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
      private readonly IRepositoryContext _context;
      public CreateTransactionCommandHandler(IRepositoryContext context)
      {
        _context = context;
      }

      public async Task<int> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
      {
        var transaction = new Transaction();
        transaction.Amount = command.Amount;
        transaction.ClientName = command.ClientName;
        transaction.Status = command.Status;
        transaction.Type = command.Type;
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChanges();
        return transaction.Id;
      }
    }
  }
}
