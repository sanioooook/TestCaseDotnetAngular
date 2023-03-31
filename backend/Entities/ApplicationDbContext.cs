using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Entities
{
	using System;

	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public DbSet<Transaction> Transactions { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}
	}
}