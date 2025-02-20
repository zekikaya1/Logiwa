using MediatR;

namespace Logiwa.Application.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public DateTime UpdatedDate { get; set; }
}
