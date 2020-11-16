using Entities.Enums;

namespace WebAPI.Features.TransactionFeatures.Queries.GetAllTransactions
{
  public class TransactionForGetAll
  {
    public int Id { get; set; }

    public EnumStatusTransaction Status { get; set; }

    public EnumTypeTransaction Type { get; set; }

    public string ClientName { get; set; }

    public double Amount { get; set; }
  }
}