using Logiwa.Web.Models;
using Mapster;

namespace Logiwa.Web.Config;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Product, ProductDto>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity);

        TypeAdapterConfig<Category, CategoryDto>.NewConfig();
        
    
    }
    
}