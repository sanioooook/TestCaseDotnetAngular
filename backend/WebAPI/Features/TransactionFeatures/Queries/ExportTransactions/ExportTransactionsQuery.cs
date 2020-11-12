using System.Collections.Generic;
using MediatR;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries.ExportTransactions
{
  public class ExportTransactionsQuery : IRequest<IEnumerable<byte>>
  {
    public SortBy SortBy { get; set; }
  }
}
