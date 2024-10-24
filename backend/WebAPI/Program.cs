using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
var angularCorsPolicy = "_angularCorsPolicy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: angularCorsPolicy, builder =>
    {
        var fronendUrl = Environment.GetEnvironmentVariable("FRONT_URL") ?? "http://localhost:4200";
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(fronendUrl, "http://localhost:5000", "https://localhost:5001");
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

LogHelper.Logger.Log(new LogEntry() { EventLogLevel = EventLogLevel.Informational, Message = $"Environment IsDevelopment: {builder.Environment.IsDevelopment()}" });
//string connection;
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
//    connection = builder.Configuration.GetConnectionString("DefaultConnection");
//}
//else
//{
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
//}
LogHelper.Logger.Log(new LogEntry(){EventLogLevel = EventLogLevel.Informational, Message = $"Current connection string is: {connection}" });

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
    options.UseSqlServer(connection));

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
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //context.Database.EnsureCreated();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseCors(angularCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();