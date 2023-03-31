using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Entities;
using Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Queries.ExportTransactions
{
  public class ExportTransactionsQueryHandler : IRequestHandler<ExportTransactionsQuery, IEnumerable<byte>>
  {

    private readonly ApplicationDbContext _context;
    public ExportTransactionsQueryHandler(ApplicationDbContext context)
    {
      _context = context;
    }
    public async Task<IEnumerable<byte>> Handle(ExportTransactionsQuery query, CancellationToken cancellationToken)
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

      var transactionList = await transactions.ToListAsync(cancellationToken);

      return await ConvertTransactionsToXlsxAsync(transactionList);
    }

    private static async Task<IEnumerable<byte>> ConvertTransactionsToXlsxAsync(IEnumerable<Transaction> transactions)
    {
      using var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("Transactions");
      var currentRow = 1;
      worksheet.Cell(currentRow, 1).Value = "TransactionId";
      worksheet.Cell(currentRow, 2).Value = "Type";
      worksheet.Cell(currentRow, 4).Value = "Status";
      worksheet.Cell(currentRow, 5).Value = "ClientName";
      worksheet.Cell(currentRow, 3).Value = "Amount";
      foreach(var transaction in transactions)
      {
        currentRow++;
        worksheet.Cell(currentRow, 1).Value = transaction.Id;
        worksheet.Cell(currentRow, 2).Value = transaction.Type.ToString();
        worksheet.Cell(currentRow, 4).Value = transaction.Status.ToString();
        worksheet.Cell(currentRow, 5).Value = transaction.ClientName;
        worksheet.Cell(currentRow, 3).Value = transaction.Amount;
      }

      await using var stream = new MemoryStream();
      workbook.SaveAs(stream);
      return stream.ToArray();
    }

  }
}