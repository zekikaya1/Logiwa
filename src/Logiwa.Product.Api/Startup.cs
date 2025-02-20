using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logiwa.Application.Commands;
using Logiwa.Application.Repositories;
using Logiwa.Application.Validators;
using Logiwa.Infrastructure.Persistence;
using Logiwa.Infrastructure.Persistence.Repositories;
using Logiwa.Product.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        var assembly = typeof(CreateProductCommandValidator).Assembly;
        Console.WriteLine($"Loading Validators from Assembly: {assembly.FullName}");
        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMapsterConfig();

        services.AddDbContext<LogiwaDbContext>(options =>
            options.UseNpgsql(Configuration["Data:DbContext:DefaultConnection"]));

        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateProductCommandValidator>());

        services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}