using MediatR;

namespace WebAPI.Features.TransactionFeatures.Queries.GetTransactionById
{
  public class GetTransactionByIdQuery : IRequest<TransactionForGetById>
  {
    public int Id { get; set; }
  }
}
