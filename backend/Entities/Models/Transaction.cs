using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Entities.Converters;
using Entities.Enums;
using FileHelpers;

namespace Entities.Models
{
  [Table("Transaction"), DelimitedRecord(","), IgnoreFirst(1)]
  public class Transaction
  {
    public int Id { get; set; }

    public EnumStatusTransaction Status { get; set; }

    public EnumTypeTransaction Type { get; set; }

    public string ClientName { get; set; }

    [FieldConverter(typeof(MoneyConverter))]
    public double Amount { get; set; }
  }
}