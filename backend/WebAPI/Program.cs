using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var angularCorsPolicy = "_angularCorsPolicy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: angularCorsPolicy, builder =>
	{
		builder
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials()
			.WithOrigins("http://localhost:4200", "http://localhost:5000", "https://localhost:5001");
	});
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "WebApi",
	});
});

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<ApplicationDbContext>();
	context.Database.EnsureCreated();
	if (context.Database.GetPendingMigrations().Any())
	{
		context.Database.Migrate();
	}
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(angularCorsPolicy);

app.Run();