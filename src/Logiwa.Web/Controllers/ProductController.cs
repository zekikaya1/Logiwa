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

    public ProductController(ApplicationDbContext context, IProductApiClient productApiClient)
    {
        _context = context;
        _productApiClient = productApiClient;
        MappingConfig.Configure();
    }

    public async Task<IActionResult> Index()
    {
        var productsResponse = await _productApiClient.GetProducts();
  
        return View(productsResponse);
    }

    public async Task<IActionResult> Search(string searchKeyword, int? minStock, int? maxStock)
    {
        var productsQuery = _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            string keyword = searchKeyword.Trim().ToLower(); // Kullanıcının girdiği kelimeyi normalize et

            productsQuery = productsQuery.Where(p =>
                p.Name.ToLower().Contains(keyword) ||
                p.Description.ToLower().Contains(keyword) ||
                p.Category.Name.ToLower().Contains(keyword));
        }

        if (minStock.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.StockQuantity >= minStock.Value);
        }

        if (maxStock.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.StockQuantity <= maxStock.Value);
        }

        ViewData["searchKeyword"] = searchKeyword;
        ViewData["minStock"] = minStock?.ToString() ?? "";
        ViewData["maxStock"] = maxStock?.ToString() ?? "";

        var products = await productsQuery
            .Select(p => p.Adapt<ProductDto>())
            .ToListAsync();

        return View("Index", products);
    }


    // GET: Product/Create
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

            _context.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        ViewBag.Categories = (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
            .Adapt<List<CategoryDto>>();

        return View(productDto);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int id)
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
    public async Task<IActionResult> Edit(int id, ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var product = productDto.Adapt<Product>();
            product.UpdatedDate = DateTime.UtcNow;

            _context.Update(product);
            await _context.SaveChangesAsync();
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
    public async Task<IActionResult> Delete(int id)
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

// POST: Product/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.IsDeleted = true;
        product.UpdatedDate = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}