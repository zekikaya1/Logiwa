using Logiwa.Web.Application.Services;
using Logiwa.Web.Config;
using Logiwa.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductApiClient _productApiClient;
    private readonly ILogger<ProductController> _logger;
    private readonly ICategoryApiClient _categoryApiClient;


    public ProductController(IProductApiClient productApiClient, ILogger<ProductController> logger,
        ICategoryApiClient categoryApiClient)
    {
        _productApiClient = productApiClient;
        _logger = logger;
        _categoryApiClient = categoryApiClient;
        MappingConfig.Configure();
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var responseProducts = await _productApiClient.GetProducts();

            _logger.LogInformation("Successfully retrieved {ProductCount} products from the database.",
                responseProducts.Count);

            return View(responseProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products: {ErrorMessage}", ex.Message);
            return View("Error");
        }
    }

    public async Task<IActionResult> Search(string searchKeyword, int? minStock, int? maxStock)
    {
        try
        {
            var products = await _productApiClient.SearchProducts(searchKeyword, minStock, maxStock);

            ViewData["searchKeyword"] = searchKeyword;
            ViewData["minStock"] = minStock?.ToString() ?? "";
            ViewData["maxStock"] = maxStock?.ToString() ?? "";

            return View("Index", products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching for products.");
            return View("Error");
        }
    }

    public async Task<IActionResult> Create()
    {
        var categoriesResponse = await _categoryApiClient.GetCategoriesAsync();
        var categories = categoriesResponse.Adapt<List<CategoryDto>>();

        ViewBag.Categories = categories;
        return View(new ProductDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var product = productDto.Adapt<Product>();
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedDate = DateTime.UtcNow;

                await _productApiClient.CreateProduct(productDto);
                _logger.LogInformation("Product created successfully.");
                return RedirectToAction(nameof(Index));
            }

            var categoriesResponse = await _categoryApiClient.GetCategoriesAsync();
            var categories = categoriesResponse.Adapt<List<CategoryDto>>();

            ViewBag.Categories = categories;
            return View(productDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating the product: {ex.Message}");
            return RedirectToAction("Index", "Error");
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        try
        {
            var productResponse = await _productApiClient.GetProductById(id);
            if (productResponse == null || productResponse.IsDeleted)
            {
                return NotFound();
            }

            var productDto = productResponse.Adapt<ProductDto>();

            var categoriesResponse = await _categoryApiClient.GetCategoriesAsync();
            var categories = categoriesResponse.Adapt<List<CategoryDto>>();

            ViewBag.Categories = categories;

            ViewBag.Categories = new SelectList(categories, "Id",
                "Name", productDto.CategoryId);

            return View(productDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating the product: {ex.Message}");
            return RedirectToAction("InternalServerError", "Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var productResponse = await _productApiClient.GetProductById(id);

            if (productResponse == null)
            {
                return NotFound();
            }

            productResponse.UpdatedDate = DateTime.UtcNow;
            await _productApiClient.UpdateProduct(id, productDto);

            return RedirectToAction(nameof(Index));
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        var categoriesResponse = await _categoryApiClient.GetCategoriesAsync();
        var categories = categoriesResponse.Adapt<List<CategoryDto>>();

        ViewBag.Categories = categories;

        return View(productDto);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var productResponse = await _productApiClient.GetProductById(id);

        if (productResponse == null)
        {
            return NotFound();
        }

        var productDto = productResponse.Adapt<ProductDto>();

        return View(productDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        try
        {
            var productResponse = await _productApiClient.GetProductById(id);
            if (productResponse == null)
            {
                return NotFound();
            }

            productResponse.IsDeleted = true;
            productResponse.UpdatedDate = DateTime.UtcNow;
            await _productApiClient.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating the product: {ex.Message}");
            return RedirectToAction("InternalServerError", "Error");
        }
    }
}