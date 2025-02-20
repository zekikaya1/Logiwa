using Logiwa.Web.Models;

namespace Logiwa.Web.Application.Services;

public interface ICategoryApiClient
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(long id);
}