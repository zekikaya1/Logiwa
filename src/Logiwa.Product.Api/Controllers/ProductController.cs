using Logiwa.Application.Commands;
using Logiwa.Application.Models.Product;
using Logiwa.Application.Queries;
using Logiwa.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Product.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly LogiwaDbContext _context;
    private readonly IMediator _mediator;
    public ProductController(LogiwaDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetProductsQuery());

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id && !p.IsDeleted)
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound(new { Message = "Product not found" });

        return Ok(product.Adapt<ProductDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        var createProductCommand = productDto.Adapt<CreateProductCommand>();
        createProductCommand.CreatedDate = DateTime.UtcNow;

        await _mediator.Send(createProductCommand);

        return CreatedAtAction(nameof(GetById), new { id = createProductCommand.Id }, createProductCommand.Adapt<ProductDto>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
    {
        var updateProductCommand = productDto.Adapt<UpdateProductCommand>();
        updateProductCommand.Id = id;
        
        await _mediator.Send(updateProductCommand);

        return Ok(new { Message = "Product updated successfully", Data = updateProductCommand });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null || product.IsDeleted)
            return NotFound(new { Message = "Product not found" });

        product.IsDeleted = true;
        product.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
