using System.Reflection;
using Logiwa.Web.Application.Configs;

namespace Logiwa.Web.Extensions;

public static class ConfigExtensions
{
    public static void AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var types = assemblies.SelectMany(x => x.GetTypes())
            .Where(t => t.IsDefined(typeof(ConfigModelAttribute))).ToList();
        
        foreach (var type in types)
        {
            var configModel =
                (ConfigModelAttribute) Attribute.GetCustomAttribute(type, typeof(ConfigModelAttribute));
            
            var methodInfo = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", new[] { typeof(IServiceCollection),typeof(IConfiguration) })!
                .MakeGenericMethod(type);

            methodInfo!.Invoke(services, new object[]{services, configuration.GetSection(configModel!.ConfigKey)});
        }
    }
}