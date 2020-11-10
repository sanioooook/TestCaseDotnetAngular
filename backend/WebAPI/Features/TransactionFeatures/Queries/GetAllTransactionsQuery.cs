using System.Collections.Generic;
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

    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Pagination<Transaction>>
    {
      private readonly IRepositoryContext _context;
      public GetAllTransactionsQueryHandler(IRepositoryContext context)
      {
        _context = context;
      }
      public async Task<Pagination<Transaction>> Handle(GetAllTransactionsQuery query, CancellationToken cancellationToken)
      {
        var transactions = await _context.Transactions
          .ToListAsync(cancellationToken);
        transactions?.AsReadOnly();
        var paginationTransaction = transactions!
          .Skip(query.Pagination.PageSize * query.Pagination.PageNumber)
          .Take(query.Pagination.PageSize)
          .ToList();

        if(query.Pagination.SortStatusBy != null || query.Pagination.SortTypeBy != null)
        {
          paginationTransaction = SortTransaction(paginationTransaction, query.Pagination);
        }

        return new Pagination<Transaction>()
        {
          TotalCount = transactions.Count,
          Data = paginationTransaction,
          PageSize = query.Pagination.PageSize,
          PageNumber = query.Pagination.PageNumber,
          PageCount = paginationTransaction.Count ,
          SortTypeBy = query.Pagination.SortTypeBy,
          SortStatusBy = query.Pagination.SortStatusBy
        };
      }

      private static List<Transaction> SortTransaction(List<Transaction> transactions, Pagination<Transaction> pagination)
      {
        if(pagination.SortStatusBy != null && pagination.SortTypeBy != null)
        {
          transactions = transactions
            .OrderByDescending((a) => a.Type == pagination.SortTypeBy && a.Status == pagination.SortStatusBy).ToList();
        }
        else
        {
          if(pagination.SortTypeBy != null)
          {
            transactions =
              transactions.OrderByDescending((a) => a.Type == pagination.SortTypeBy).ToList();
          }
          if(pagination.SortStatusBy != null)
          {
            transactions =
              transactions.OrderByDescending((a) => a.Status == pagination.SortStatusBy).ToList();
          }
        }
        return transactions;
      }
    }
  }
}