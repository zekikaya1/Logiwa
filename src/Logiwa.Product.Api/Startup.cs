using Logiwa.Application.Commands;
using Logiwa.Application.Repositories;
using Logiwa.Infrastructure.Persistence;
using Logiwa.Infrastructure.Persistence.Repositories;
using Logiwa.Product.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Product.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {           
 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMapsterConfig();

        services.AddDbContext<LogiwaDbContext>(options =>
            options.UseNpgsql(Configuration["Data:DbContext:DefaultConnection"]));
        
        services.AddTransient<IProductRepository, ProductRepository>();
    }

    public  void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}