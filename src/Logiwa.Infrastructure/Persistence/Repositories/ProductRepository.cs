using System.Linq.Expressions;
using Logiwa.Application.Repositories;
using Logiwa.Core.Entities;

namespace Logiwa.Infrastructure.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(LogiwaDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> HasAnyProductByName(string productName)
    {
        var result = await Get(
            ApplyBusinessRules(x => x.Name.ToLower() == productName.ToLower()),
            cancellationToken: default
        );
        return result is { Count: > 0 };
    }

    public async Task<bool> HasAnyProductById(long productId)
    {
        var result = await Get(
            ApplyBusinessRules(x => x.Id == productId),
            cancellationToken: default
        );
        return result is { Count: > 0 };
    }


    public async Task<Product> GetProductById(long productId, CancellationToken cancellationToken)
    {
        return await GetSingleAsync(
            ApplyBusinessRules(p => p.Id == productId),
            "Category",
            cancellationToken
        );
    }

    public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await Get(
            ApplyBusinessRules(p => true),
            null,
            "Category",
            cancellationToken
        );
    }

    public async Task<List<Product>> GetProductsByStockRange(int? minStock, int? maxStock,
        CancellationToken cancellationToken)
    {
        return await Get(
            ApplyBusinessRules(p =>
                (!minStock.HasValue || p.StockQuantity >= minStock.Value) &&
                (!maxStock.HasValue || p.StockQuantity <= maxStock.Value)
            ),
            null,
            "Category",
            cancellationToken
        );
    }


    public async Task<List<Product>> SearchProducts(string keyword, CancellationToken cancellationToken)
    {
        return await Get(
            ApplyBusinessRules(p =>
                p.Name.ToLower().Contains(keyword.ToLower()) ||
                p.Description.ToLower().Contains(keyword.ToLower()) ||
                p.Category.Name.ToLower().Contains(keyword.ToLower())
            ),
            null,
            "Category",
            cancellationToken
        );
    }

    /// <summary>
    /// Apply business rules
    /// Exclude soft deleted products
    /// Include category relationship
    /// Products with stock below category's minimum stock cannot be live
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    private static Expression<Func<Product, bool>> ApplyBusinessRules(Expression<Func<Product, bool>> predicate)
    {
        Expression<Func<Product, bool>> businessRules = p => !p.IsDeleted && p.StockQuantity >= p.Category.MinQuantity;

        return Expression.Lambda<Func<Product, bool>>(
            Expression.AndAlso(businessRules.Body, Expression.Invoke(predicate, businessRules.Parameters[0])),
            businessRules.Parameters
        );
    }
}