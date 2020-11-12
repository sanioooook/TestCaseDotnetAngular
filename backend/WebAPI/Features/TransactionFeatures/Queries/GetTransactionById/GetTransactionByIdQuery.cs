using Entities.Models;
using MediatR;

namespace WebAPI.Features.TransactionFeatures.Queries.GetTransactionById
{
  public class GetTransactionByIdQuery : IRequest<Transaction>
  {
    public int Id { get; set; }
  }
}
