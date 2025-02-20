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
   private readonly IMediator _mediator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IMediator mediator, ILogger<ProductController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _mediator.Send(new GetProductsQuery());

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching for products.");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? searchKeyword, [FromQuery] int? minStock, [FromQuery] int? maxStock)
    {
        try
        {
            var products = await _mediator.Send(new GetProductsQueryBySearch(searchKeyword, minStock, maxStock));
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching for products.");
            return StatusCode(500, "Internal Server Error");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var products = await _mediator.Send(new GetQueryProductById(id));
     
        if (products == null)
            return NotFound(new { Message = "Product not found" });

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto productDto)
    {
        var createProductCommand = productDto.Adapt<CreateProductCommand>();

        await _mediator.Send(createProductCommand);

        return CreatedAtAction(nameof(GetById), new { id = createProductCommand.Id },
            createProductCommand.Adapt<ProductDto>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] ProductDto productDto)
    {
        var updateProductCommand = productDto.Adapt<UpdateProductCommand>();
        updateProductCommand.Id = id;

        await _mediator.Send(updateProductCommand);

        return Ok(new { Message = "Product updated successfully", Data = updateProductCommand });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var products = await _mediator.Send(new GetQueryProductById(id));
     
        if (products == null || products.IsDeleted)
            return NotFound(new { Message = "Product not found" });

        products.IsDeleted = true;
        products.UpdatedDate = DateTime.UtcNow;

        var deleteProductCommand = products.Adapt<DeleteProductCommand>();
        deleteProductCommand.Id = id;
        await _mediator.Send(deleteProductCommand);
        return NoContent();
    }
}