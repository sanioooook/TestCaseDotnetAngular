namespace Entities;

using Microsoft.EntityFrameworkCore;
using Models;

public interface IApplicationDbContext
{
	public DbSet<Transaction> Transactions { get; set; }
}