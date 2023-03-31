namespace Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		const string defaultCs = "Server=localhost;Database=TestCaseDotnetAngular;Trusted_Connection=True;MultipleActiveResultSets=true";
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

		optionsBuilder.UseSqlServer(args.Any() ? args[0] : defaultCs);

		return new ApplicationDbContext(optionsBuilder.Options);
		// dotnet ef migrations add InitialCreate --project Entities
	}
}