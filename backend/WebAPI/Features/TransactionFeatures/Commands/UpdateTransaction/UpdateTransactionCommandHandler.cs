using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Commands.UpdateTransaction
{
  public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, int>
  {
    private readonly RepositoryContext _context;

    public UpdateTransactionCommandHandler(RepositoryContext context)
    {
      _context = context;
    }

    public async Task<int> Handle(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
      var transaction = await _context.Transactions
        .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
      if(transaction == null)
      {
        return 0;
      }
      transaction.Status = command.Status;
      transaction.Amount = command.Amount;
      transaction.ClientName = command.ClientName;
      transaction.Type = command.Type;
      await _context.SaveChangesAsync(cancellationToken);
      return transaction.Id;
    }
  }
}