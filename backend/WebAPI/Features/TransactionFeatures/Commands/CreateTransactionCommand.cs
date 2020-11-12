using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Entities.Converters;
using Entities.Models;
using MediatR;
using WebAPI.Attributes.Validators;
using Entities.Enums;
using FileHelpers;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Features.TransactionFeatures.Commands
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

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
      private readonly RepositoryContext _context;
      public CreateTransactionCommandHandler(RepositoryContext context)
      {
        _context = context;
      }

      public async Task<int> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
      {
        var transaction = new Transaction
        {
          Amount = command.Amount,
          ClientName = command.ClientName,
          Status = command.Status,
          Type = command.Type
        };

        var findTransaction = await _context.Transactions.Where(val =>
            val.Type == transaction.Type && 
            val.ClientName == transaction.ClientName &&
            val.Status == transaction.Status && 
            Math.Abs(val.Amount - transaction.Amount) == 0)
          .FirstOrDefaultAsync(cancellationToken);
        if(findTransaction != null)
        {
          return 0;
        }
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
      }
    }
  }
}
