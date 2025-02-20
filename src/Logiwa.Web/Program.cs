using CorrelationId.DependencyInjection;
using Logiwa.Web.Extensions;

namespace Logiwa.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClients(builder.Configuration);
        
        builder.Services.AddDefaultCorrelationId((s) =>
        {
            s.RequestHeader = "x-correlationid";
            s.UpdateTraceIdentifier = true;
        });
           
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}