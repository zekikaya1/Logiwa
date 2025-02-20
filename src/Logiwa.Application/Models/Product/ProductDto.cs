using System.ComponentModel.DataAnnotations;

namespace Logiwa.Application.Models.Product;

public class ProductDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}