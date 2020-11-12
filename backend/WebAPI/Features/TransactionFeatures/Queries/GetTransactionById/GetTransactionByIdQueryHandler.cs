using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Queries.GetTransactionById
{
  public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
  {
    private readonly RepositoryContext _context;
    public GetTransactionByIdQueryHandler(RepositoryContext context)
    {
      _context = context;
    }
    public async Task<Transaction> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
      var transaction = await _context.Transactions.FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);
      return transaction;
    }
  }
}