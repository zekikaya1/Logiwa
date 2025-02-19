using Logiwa.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Logiwa.Web.Services;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    DbSet<Product> Products { get; set; }
    Task<int> SaveChangesAsync();
}