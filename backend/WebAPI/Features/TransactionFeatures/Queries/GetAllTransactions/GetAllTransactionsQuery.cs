using Entities.Models;
using MediatR;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries.GetAllTransactions
{
  public class GetAllTransactionsQuery : IRequest<Pagination<Transaction>>
  {
    public Pagination<Transaction> Pagination { get; set; }

    public SortBy SortBy { get; set; }
  }
}