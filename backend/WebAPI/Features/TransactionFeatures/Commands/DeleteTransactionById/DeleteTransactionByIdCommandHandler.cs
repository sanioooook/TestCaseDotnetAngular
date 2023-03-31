using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Commands.DeleteTransactionById
{
  public class DeleteTransactionByIdCommandHandler : IRequestHandler<DeleteTransactionByIdCommand, int>
  {
    private readonly ApplicationDbContext _context;
    public DeleteTransactionByIdCommandHandler(ApplicationDbContext context)
    {
      _context = context;
    }
    public async Task<int> Handle(DeleteTransactionByIdCommand command, CancellationToken cancellationToken)
    {
      var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
      if(transaction == null)
      {
        return 0;
      }

      _context.Transactions.Remove(transaction);
      await _context.SaveChangesAsync(cancellationToken);
      return transaction.Id;
    }
  }
}