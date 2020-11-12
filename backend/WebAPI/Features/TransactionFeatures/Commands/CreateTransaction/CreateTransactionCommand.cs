using System.ComponentModel.DataAnnotations;
using Entities.Converters;
using Entities.Enums;
using FileHelpers;
using MediatR;
using WebAPI.Attributes.Validators;

namespace WebAPI.Features.TransactionFeatures.Commands.CreateTransaction
{
  [DelimitedRecord(","), IgnoreFirst(1)]
  public class CreateTransactionCommand: IRequest<int>
  {
    public int Id { get; set; }

    [Required, ValidEnum]
    public EnumStatusTransaction Status { get; set; }

    [Required, ValidEnum]
    public EnumTypeTransaction Type { get; set; }

    [Required]
    public string ClientName { get; set; }

    [Required, Range(0d, double.MaxValue), FieldConverter(typeof(MoneyConverter))]
    public double Amount { get; set; }
  }
}
