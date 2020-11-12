using System.ComponentModel.DataAnnotations;
using Entities.Enums;
using MediatR;
using WebAPI.Attributes.Validators;

namespace WebAPI.Features.TransactionFeatures.Commands.UpdateTransaction
{
  public class UpdateTransactionCommand : IRequest<int>
  {
    [Required]
    public int Id { get; set; }

    [Required, ValidEnum]
    public EnumStatusTransaction Status { get; set; }

    [Required, ValidEnum]
    public EnumTypeTransaction Type { get; set; }

    [Required]
    public string ClientName { get; set; }

    [Required, Range(0d, double.MaxValue)]
    public double Amount { get; set; }

  }
}