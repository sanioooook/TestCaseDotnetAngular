using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Enums;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries
{
  public class ExportTransactionsQuery : IRequest<IEnumerable<Transaction>>
  {
    public EnumStatusTransaction? SortStatusTransaction { get; set; }
    public EnumTypeTransaction? SortTypeTransaction { get; set; }
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
        if(pagination.SortStatusTransaction != null && pagination.SortTypeTransaction != null)
        {
          transactions = transactions
            .OrderByDescending((a) => a.Type == pagination.SortTypeTransaction && a.Status == pagination.SortStatusTransaction)
            .ToList();
        }
        else
        {
          if(pagination.SortTypeTransaction != null)
          {
            transactions =
              transactions.OrderByDescending((a) => a.Type == pagination.SortTypeTransaction).ToList();
          }
          if(pagination.SortStatusTransaction != null)
          {
            transactions =
              transactions.OrderByDescending((a) => a.Status == pagination.SortStatusTransaction).ToList();
          }
        }
        return transactions;
      }
    }
  }
}