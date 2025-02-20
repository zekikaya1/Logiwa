using System.Net;
using Logiwa.Web.Application.Services;
using Logiwa.Web.Models;
using Newtonsoft.Json;

namespace Logiwa.Web.Infrastructure.Clients;

public class CategoryApiClient : ICategoryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public CategoryApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var uri = _configuration.GetSection("CategoryApiClient:EndPoints:GetCategories").Value!;
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception(
                $"CategoryApiClient get request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<CategoryDto>>(content);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(long id)
    {
        var uriTemplate = _configuration.GetSection("CategoryApiClient:EndPoints:GetCategoryById").Value!;
        var uri = string.Format(uriTemplate, id);

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (response.StatusCode != HttpStatusCode.OK)
            throw new Exception(
                $"CategoryApiClient get by id request failed. Status: {(int)response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CategoryDto>(content);
    }
}