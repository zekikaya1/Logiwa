using Logiwa.Web.Models;

namespace Logiwa.Web.Application.Services;

public interface IProductApiClient
{
    public Task<List<ProductDto>> GetProducts();
}