using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Enums;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Queries
{
  public class ExportTransactionsQuery : IRequest<IEnumerable<Transaction>>
  {
    public EnumStatusTransaction? SortStatusBy { get; set; }
    public EnumTypeTransaction? SortTypeBy { get; set; }
    public class ExportTransactionsQueryHandler : IRequestHandler<ExportTransactionsQuery, IEnumerable<Transaction>>
    {

      private readonly IRepositoryContext _context;
      public ExportTransactionsQueryHandler(IRepositoryContext context)
      {
        _context = context;
      }
      public async Task<IEnumerable<Transaction>> Handle(ExportTransactionsQuery query, CancellationToken cancellationToken)
      {
        var transactions = await _context.Transactions.ToListAsync(cancellationToken);
        transactions?.AsReadOnly();
        return SortTransaction(transactions, query);
      }

      private static IEnumerable<Transaction> SortTransaction(IEnumerable<Transaction> transactions, ExportTransactionsQuery pagination)
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
        return transactions;
      }
    }
  }
}