using Logiwa.Application.Models.Product;
using MediatR;

namespace Logiwa.Application.Commands;

public class CreateProductCommand :IRequest<Unit>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}