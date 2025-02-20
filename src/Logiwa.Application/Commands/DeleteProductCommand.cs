using MediatR;

namespace Logiwa.Application.Commands;

public class DeleteProductCommand :IRequest<bool>
{
    public long Id { get; set; }
}