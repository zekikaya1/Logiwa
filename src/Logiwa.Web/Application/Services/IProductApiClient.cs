using Logiwa.Web.Models;

namespace Logiwa.Web.Application.Services;

public interface IProductApiClient
{
    public Task<List<ProductDto>> GetProducts();
    public Task CreateProduct(ProductDto productDto);
    public Task UpdateProduct(long id, ProductDto productDto);
    public Task DeleteProduct(long id);
    public Task<ProductDto> GetProductById(long id);
    public Task<List<ProductDto>> SearchProducts(string? searchKeyword, int? minStock, int? maxStock);
}