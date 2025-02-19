using Logiwa.Application.Repositories;
using Logiwa.Core.Entities;

namespace Logiwa.Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.Configuration;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(LogiwaDbContext dbContext, IConfiguration configuration)
        : base(dbContext)
    {
        _configuration = configuration;
    }

    public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await Get(
            p => !p.IsDeleted,null,
            "Category",
            cancellationToken);
    }

    public async Task<Product> GetProductById(long productId, CancellationToken cancellationToken)
    {
        return await GetSingleAsync(
            p => p.Id.Equals(productId) && !p.IsDeleted,
            "Category",
            cancellationToken);
    }

    public async Task<List<Product>> GetProductsByCategoryId(long categoryId, CancellationToken cancellationToken)
    {
        return await Get(
            p => p.CategoryId == categoryId && !p.IsDeleted,
            cancellationToken: cancellationToken);
    }

    public async Task<List<Product>> GetProductsByStockRange(int minStock, int maxStock,
        CancellationToken cancellationToken)
    {
        return await Get(
            p => p.StockQuantity >= minStock && p.StockQuantity <= maxStock && !p.IsDeleted,
            cancellationToken: cancellationToken);
    }


    public async Task<List<Product>> SearchProducts(string keyword, CancellationToken cancellationToken)
    {
        return await Get(
            p => !p.IsDeleted &&
                 p.Name.ToLower().Contains(keyword.ToLower()) ||
                 p.Description.ToLower().Contains(keyword.ToLower()) ||
                 p.Category.Name.ToLower().Contains(keyword.ToLower()), null, "Category",
            cancellationToken);
    }

    public async Task<int> ProductCountByCategoryId(long categoryId, CancellationToken cancellationToken)
    {
        return await Count(
            p => p.CategoryId == categoryId && !p.IsDeleted,
            cancellationToken: cancellationToken);
    }
}