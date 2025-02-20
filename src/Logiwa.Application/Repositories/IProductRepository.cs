using Logiwa.Core.Entities;

namespace Logiwa.Application.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<bool> HasAnyProductByName(string productName);
    public Task<bool> HasAnyProductById(long productId);

    public Task<Product> GetProductById(long productId, CancellationToken cancellationToken);

    public Task<List<Product>> GetProducts(CancellationToken cancellationToken);

    public Task<List<Product>> GetProductsByStockRange(int? minStock, int? maxStock,
        CancellationToken cancellationToken);

    Task<List<Product>> SearchProducts(string keyword, CancellationToken cancellationToken);
}