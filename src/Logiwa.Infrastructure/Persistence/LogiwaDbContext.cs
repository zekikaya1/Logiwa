using Logiwa.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Infrastructure.Persistence;

public class LogiwaDbContext : DbContext
{
    public LogiwaDbContext(DbContextOptions<LogiwaDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogiwaDbContext).Assembly);

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
    }
}
