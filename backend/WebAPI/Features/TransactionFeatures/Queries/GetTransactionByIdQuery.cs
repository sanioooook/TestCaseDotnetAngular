using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Queries
{
  public class GetTransactionByIdQuery : IRequest<Transaction>
  {
    public int Id { get; set; }
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
      private readonly IRepositoryContext _context;
      public GetTransactionByIdQueryHandler(IRepositoryContext context)
      {
        _context = context;
      }
      public async Task<Transaction> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
      {
        var product = _context.Transactions.FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);
        return await product;
      }
    }
  }
}
