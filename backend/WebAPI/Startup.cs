using System.Reflection;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
  public class Startup
  {
    private const string AngularCorsPolicy = "_angularCorsPolicy";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(name: AngularCorsPolicy, builder =>
        {
          builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200");
        });
      });
      services.AddDbContext<IRepositoryContext, RepositoryContext>(options =>
        options.UseSqlServer(
          Configuration.GetConnectionString("DefaultConnection"),
          b => b.MigrationsAssembly(typeof(RepositoryContext).Assembly.FullName)));
      services.AddMediatR(Assembly.GetExecutingAssembly());
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Version = "v1",
          Title = "WebApi",
        });
      });
      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if(env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(AngularCorsPolicy);

      //app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
