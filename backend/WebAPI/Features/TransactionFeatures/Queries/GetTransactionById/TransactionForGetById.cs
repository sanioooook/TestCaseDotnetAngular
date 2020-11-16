using Entities.Enums;

namespace WebAPI.Features.TransactionFeatures.Queries.GetTransactionById
{
  public class TransactionForGetById
  {
    public int Id { get; set; }

    public EnumStatusTransaction Status { get; set; }

    public EnumTypeTransaction Type { get; set; }

    public string ClientName { get; set; }

    public double Amount { get; set; }
  }
}
