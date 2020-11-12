using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Commands
{

  public class DeleteTransactionByIdCommand : IRequest<int>
  {
    [Required]
    public int Id { get; set; }

    public class DeleteTransactionByIdCommandHandler : IRequestHandler<DeleteTransactionByIdCommand, int>
    {
      private readonly RepositoryContext _context;
      public DeleteTransactionByIdCommandHandler(RepositoryContext context)
      {
        _context = context;
      }
      public async Task<int> Handle(DeleteTransactionByIdCommand command, CancellationToken cancellationToken)
      {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);
        if(transaction == null)
        {
          return default;
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
      }
    }
  }


  
}