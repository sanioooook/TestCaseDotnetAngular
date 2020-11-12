using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries
{
  public class GetAllTransactionsQuery : IRequest<Pagination<Transaction>>
  {
    public Pagination<Transaction> Pagination { get; set; }

    public SortBy SortBy { get; set; }

    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Pagination<Transaction>>
    {
      private readonly RepositoryContext _context;
      public GetAllTransactionsQueryHandler(RepositoryContext context)
      {
        _context = context;
      }
      public async Task<Pagination<Transaction>> Handle(GetAllTransactionsQuery query, CancellationToken cancellationToken)
      {
        var transactions = _context.Transactions.Select(transaction => transaction);

        if(query.SortBy != null)
        {
          if(query.SortBy.SortStatusBy != null && query.SortBy.SortTypeBy != null)
          {
            transactions =
              transactions.OrderByDescending(a => a.Type == query.SortBy.SortTypeBy)
                .ThenByDescending(a => a.Status == query.SortBy.SortStatusBy);
          }
          else
          {
            if(query.SortBy.SortTypeBy != null)
            {
              transactions =
                transactions.OrderByDescending(a => a.Type == query.SortBy.SortTypeBy);
            }
            if(query.SortBy.SortStatusBy != null)
            {
              transactions =
                transactions.OrderByDescending(a => a.Status == query.SortBy.SortStatusBy);
            }
          }
        }

        transactions = transactions.Skip(query.Pagination.PageSize * query.Pagination.PageNumber)
          .Take(query.Pagination.PageSize);

        var paginationTransaction = await transactions.ToListAsync(cancellationToken);
        return new Pagination<Transaction>()
        {
          TotalCount = _context.Transactions.Count(),
          Data = paginationTransaction,
          PageSize = query.Pagination.PageSize,
          PageNumber = query.Pagination.PageNumber,
          PageCount = paginationTransaction.Count
        };
      }
    }
  }
}