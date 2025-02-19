using Logiwa.Web.Application.Services;
using Logiwa.Web.Infrastructure.Clients;
using Logiwa.Web.Infrastructure.Clients.Configs;
using Logiwa.Web.Infrastructure.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Logiwa.Web.Extensions
{
    public static class ServiceCollectionHttpClientExtensions
    {
        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<AgentNameDelegatingHandler>();
            services.AddTransient<CorrelationIdDelegatingHandler>();
            
            services.AddConfigs(configuration);
            
            var productApiClientConfig = configuration.GetSection("ProductApiClient").Get<ClientConfig>();
            services.AddHttpClient<IProductApiClient, ProductApiClient>(
                productApiClientConfig.BaseAddress,
                productApiClientConfig.Timeout,
                productApiClientConfig.RetryCount,
                productApiClientConfig.RetryDelayInMs);

            services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
        }

        private static void AddHttpClient<TClient, TImplementation>(this IServiceCollection services,
            string baseAddress, int timeoutInMs, int retryCount, int retryIntervalInMs)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(client => client.BaseAddress = new Uri(baseAddress))
                .AddHttpMessageHandler<AgentNameDelegatingHandler>()
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
                .AddPolicyHandler(
                    Policy.TimeoutAsync<HttpResponseMessage>(
                        TimeSpan.FromMilliseconds(timeoutInMs)))
                .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(retryCount,
                        _ => TimeSpan.FromMilliseconds(retryIntervalInMs)))
                .SetHandlerLifetime(TimeSpan.FromMinutes(30));
        }
    }
}