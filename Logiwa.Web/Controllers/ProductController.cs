
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logiwa.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logiwa.Web.Config;
using Logiwa.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
        MappingConfig.Configure(); // Mapster Konfigürasyonu burada uygulanacak
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        var productDtos = products.Adapt<List<ProductDto>>(); // Mapster Adapt kullanımı
        return View(productDtos);
    }

    // GET: Product/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories =
            (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
            .Adapt<List<CategoryDto>>(); // Mapster Adapt kullanımı
        return View(new ProductDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            var product = productDto.Adapt<Product>(); // Mapster Adapt kullanımı
            product.CreatedDate = DateTime.UtcNow;
            product.UpdatedDate = DateTime.UtcNow;

            _context.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Kategorileri ViewBag'e atıyoruz
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

        var productDto = product.Adapt<ProductDto>(); // Mapster Adapt kullanımı
        /*ViewBag.Categories =
            (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
            .Adapt<List<CategoryDto>>(); // Mapster Adapt kullanımı*/
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
            var product = productDto.Adapt<Product>(); // Mapster Adapt kullanımı
            product.UpdatedDate = DateTime.UtcNow;

            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); // Konsola hataları yazdırın veya loglayın
            }

            ViewBag.Categories =
                (await _context.Categories.Where(c => !c.IsDeleted).ToListAsync())
                .Adapt<List<CategoryDto>>(); // Mapster Adapt kullanımı
        }

        return View(productDto);
    }


    /*// POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
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

        _context.Update(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }*/

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

        var productDto = product.Adapt<ProductDto>(); // DTO'ya dönüştürülmesi
        return View(productDto); // Silme sayfasını yükle
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
        _context.Products.Update(product); // Ürünü sil
        await _context.SaveChangesAsync(); // Değişiklikleri kaydet

        return RedirectToAction(nameof(Index)); // Silme işleminden sonra listeye dön
    }
}