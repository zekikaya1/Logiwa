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

        public ProductApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        
        public async Task<List<ProductDto>> GetProducts()
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:GetProducts").Value!;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(
                    $"ProductApiClient get request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductDto>>(content);
        }
        
        public async Task<ProductDto> GetProductById(long id)
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:GetProductById").Value!;
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(uri, id));

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(
                    $"ProductApiClient get request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductDto>(content);
        }
        
        public async Task CreateProduct(ProductDto productDto)
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:CreateProduct").Value!;
            var content = new StringContent(JsonConvert.SerializeObject(productDto), System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception(
                    $"ProductApiClient create request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
        }
        

        public async Task UpdateProduct(long id, ProductDto productDto)
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:UpdateProduct").Value!;
            var content = new StringContent(JsonConvert.SerializeObject(productDto), System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, string.Format(uri, id))
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(
                    $"ProductApiClient update request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<List<ProductDto>> SearchProducts(string? searchKeyword, int? minStock, int? maxStock)
        {
            var baseUri = _configuration.GetSection("ProductApiClient:EndPoints:SearchProducts").Value!;

            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(searchKeyword))
                queryParams.Add($"searchKeyword={Uri.EscapeDataString(searchKeyword)}");
            if (minStock.HasValue)
                queryParams.Add($"minStock={minStock.Value}");
            if (maxStock.HasValue)
                queryParams.Add($"maxStock={maxStock.Value}");

            var fullUri = queryParams.Count > 0 ? $"{baseUri}?{string.Join("&", queryParams)}" : baseUri;

            var response = await _httpClient.GetAsync(fullUri);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"ProductApiClient search request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductDto>>(content);
        }
        
        public async Task DeleteProduct(long id)
        {
            var uri = _configuration.GetSection("ProductApiClient:EndPoints:DeleteProduct").Value!;
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format(uri, id));

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
                throw new Exception(
                    $"ProductApiClient delete request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
