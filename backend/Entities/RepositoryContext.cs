using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
  public interface IRepositoryContext
  {
    DbSet<Transaction> Transactions { get; set; }

    Task<int> SaveChanges();
  }

  public class RepositoryContext : DbContext, IRepositoryContext
  {
    public DbSet<Transaction> Transactions { get; set; }

    public RepositoryContext(DbContextOptions options) : base(options)
    {
    }

    public async Task<int> SaveChanges()
    {
      return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
      {
        relationship.DeleteBehavior = DeleteBehavior.Restrict;
      }
    }
  }
}
