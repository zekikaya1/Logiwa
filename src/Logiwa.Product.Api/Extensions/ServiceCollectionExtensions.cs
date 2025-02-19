using Logiwa.Application.Mapper;
using Mapster;
using MapsterMapper;

namespace Logiwa.Product.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Apply(new ProductMapperConfig());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}