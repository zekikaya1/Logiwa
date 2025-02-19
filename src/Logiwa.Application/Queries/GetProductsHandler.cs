
using MediatR;
using Logiwa.Application.Models.Product;
using Logiwa.Application.Repositories;
using Mapster;

namespace Logiwa.Application.Queries;
public class GetProductsQuery : IRequest<List<ProductDto>>
{

}

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProducts(cancellationToken: cancellationToken);

        var productResponses = products.Adapt<List<ProductDto>>();
        
        return productResponses;
    }
}
