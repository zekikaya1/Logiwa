using Logiwa.Application.Models.Product;
using Logiwa.Application.Repositories;
using Mapster;
using MediatR;

namespace Logiwa.Application.Queries;

public class GetQueryProductById : IRequest<ProductDto>
{
    public long Id { get; set; }

    public GetQueryProductById(long id)
    {
        Id = id;
    }
}

public class GetQueryProductByIdHandler : IRequestHandler<GetQueryProductById, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetQueryProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetQueryProductById request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(request.Id, cancellationToken);

      var productResponse = product.Adapt<ProductDto>();

        return productResponse;
    }
}
