﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries.GetAllTransactions
{
  public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Pagination<TransactionForGetAll>>
  {
    private readonly ApplicationDbContext _context;
    public GetAllTransactionsQueryHandler(ApplicationDbContext context)
    {
      _context = context;
    }
    public async Task<Pagination<TransactionForGetAll>> Handle(GetAllTransactionsQuery query, CancellationToken cancellationToken)
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

      var transactionForGetAll = (from transaction in await transactions.ToListAsync(cancellationToken)
      select new TransactionForGetAll()
      {
        Type = transaction.Type,
        Status = transaction.Status,
        ClientName = transaction.ClientName,
        Amount = transaction.Amount,
        Id = transaction.Id
      }).ToList();

      return new Pagination<TransactionForGetAll>()
      {
        TotalCount = _context.Transactions.Count(),
        Data = transactionForGetAll,
        PageSize = query.Pagination.PageSize,
        PageNumber = query.Pagination.PageNumber,
        PageCount = transactionForGetAll.Count
      };
    }
  }
}