using Logiwa.Core.Entities;

namespace Logiwa.Application.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<List<Product>> GetProducts(CancellationToken cancellationToken);
    Task<Product> GetProductById(long productId, CancellationToken cancellationToken);
    
    Task<List<Product>> GetProductsByCategoryId(long categoryId, CancellationToken cancellationToken);
    
    Task<List<Product>> GetProductsByStockRange(int minStock, int maxStock, CancellationToken cancellationToken);
    
    Task<List<Product>> SearchProducts(string keyword, CancellationToken cancellationToken);
    
    Task<int> ProductCountByCategoryId(long categoryId, CancellationToken cancellationToken);

}
