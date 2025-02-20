using Logiwa.Application.Models.Product;
using MediatR;

namespace Logiwa.Application.Commands;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
