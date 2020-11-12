using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
  public class RepositoryContext : DbContext
  {
    public DbSet<Transaction> Transactions { get; set; }

    public RepositoryContext(DbContextOptions options) : base(options)
    {
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
