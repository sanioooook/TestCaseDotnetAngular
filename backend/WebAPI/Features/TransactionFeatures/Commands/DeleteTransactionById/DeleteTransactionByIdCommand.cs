using System.ComponentModel.DataAnnotations;
using MediatR;

namespace WebAPI.Features.TransactionFeatures.Commands.DeleteTransactionById
{

  public class DeleteTransactionByIdCommand : IRequest<int>
  {
    [Required]
    public int Id { get; set; }
  }
}