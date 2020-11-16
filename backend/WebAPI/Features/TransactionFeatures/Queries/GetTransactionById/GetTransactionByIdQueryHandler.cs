using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Queries.GetTransactionById
{
  public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionForGetById>
  {
    private readonly RepositoryContext _context;
    public GetTransactionByIdQueryHandler(RepositoryContext context)
    {
      _context = context;
    }
    public async Task<TransactionForGetById> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
      var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);
      return transaction != null
        ? new TransactionForGetById()
        {
          Type = transaction.Type,
          Status = transaction.Status,
          ClientName = transaction.ClientName,
          Amount = transaction.Amount,
          Id = transaction.Id
        }
        : null;
    }
  }
}
