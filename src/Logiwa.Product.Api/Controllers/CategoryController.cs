using Logiwa.Application.Models.Category;
using Logiwa.Application.Queries;
using Logiwa.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Product.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly LogiwaDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(LogiwaDbContext context, IMediator mediator, ILogger<CategoryController> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving categories.");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id && !c.IsDeleted)
            .FirstOrDefaultAsync();

        if (category == null)
            return NotFound(new { Message = "Category not found" });

        return Ok(category.Adapt<CategoryDto>());
    }
}