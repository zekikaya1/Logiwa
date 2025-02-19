using Logiwa.Application.Commands;
using MediatR;

namespace Logiwa.Application.Handler;

public class CreateProductCommandHandler:IRequestHandler<CreateProductCommand>
{
    public Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}