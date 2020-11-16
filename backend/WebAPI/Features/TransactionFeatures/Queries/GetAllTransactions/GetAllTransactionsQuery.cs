using MediatR;
using WebAPI.Classes;

namespace WebAPI.Features.TransactionFeatures.Queries.GetAllTransactions
{
  public class GetAllTransactionsQuery : IRequest<Pagination<TransactionForGetAll>>
  {
    public Pagination<TransactionForGetAll> Pagination { get; set; }

    public SortBy SortBy { get; set; }
  }
}