using Logiwa.Web.Application.Services;
using Logiwa.Web.Config;
using Logiwa.Web.Models;
using Logiwa.Web.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Web.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IProductApiClient _productApiClient;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(ApplicationDbContext context, IProductApiClient productApiClient, ILogger<ProductController> logger)
    {
        _context = context;
        _productApiClient = productApiClient;
        _logger = logger;
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
        ViewBag.Categories =
            (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
            .Adapt<List<CategoryDto>>();
        return View(new ProductDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto productDto)
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


        ViewBag.Categories = (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
            .Adapt<List<CategoryDto>>();

        return View(productDto);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.IsDeleted)
        {
            return NotFound();
        }

        var productDto = product.Adapt<ProductDto>();

        ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id",
            "Name", productDto.CategoryId);

        return View(productDto);
    }

    // POST: Product/Edit/5
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
             await _productApiClient.UpdateProduct(id,productDto);
            
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            ViewBag.Categories =
                (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
                .Adapt<List<CategoryDto>>();
        }

        return View(productDto);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var productDto = product.Adapt<ProductDto>();
        return View(productDto);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var productResponse = await _productApiClient.GetProductById(id);
        if (productResponse == null)
        {
            return NotFound();
        }

        productResponse.IsDeleted = true;
        productResponse.UpdatedDate = DateTime.UtcNow;
        await _productApiClient.UpdateProduct(id,productResponse);


        return RedirectToAction(nameof(Index));
    }
}