using System.Net;
using Logiwa.Web.Application.Services;
using Logiwa.Web.Models;
using Newtonsoft.Json;

namespace Logiwa.Web.Infrastructure.Clients
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductApiClient(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:GetProducts").Value!;
           
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(uri));

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode is not HttpStatusCode.OK)
                throw new Exception(
                    $"ProductApiClient get request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ProductDto>>(content);
        }
    }
}