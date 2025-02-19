using Logiwa.Web.Config;
using Logiwa.Web.Models;
using Logiwa.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // builder.Services.AddDbContext<ApplicationDbContext>(options =>
        // {
        //     var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
        //     options.UseSqlServer(connectionstring);
        // });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration["Data:DbContext:DefaultConnection"]));

 
        /*builder.Services.AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration["Data:DbContext:DefaultConnection"]));*/

        //     AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        var app = builder.Build();

        // using (var scope = app.Services.CreateScope())
        // {
        //     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //     
        //     if (context.Database.GetPendingMigrations().Any()) // Sadece yeni migration varsa uygula
        //     {
        //         context.Database.Migrate();
        //     }
        // }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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