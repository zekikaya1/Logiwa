using Logiwa.Application.Models.Product;
using Logiwa.Core.Entities;
using Mapster;

namespace Logiwa.Application.Mapper;

public class ProductMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(d => d.CategoryName, s => s.Category.Name)
            .Map(d => d.StockQuantity, s => s.StockQuantity);
    }
    
}