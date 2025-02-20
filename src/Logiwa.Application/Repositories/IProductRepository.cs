using Logiwa.Core.Entities;

namespace Logiwa.Application.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<List<Product>> GetProducts(CancellationToken cancellationToken);
    
    Task<List<Product>> GetProductsByStockRange(int minStock, int maxStock, CancellationToken cancellationToken);
    
    Task<List<Product>> SearchProducts(string keyword, CancellationToken cancellationToken);

}