using MediatR;
using Logiwa.Application.Models.Product;
using Logiwa.Application.Repositories;
using Logiwa.Core.Entities;
using Mapster;

namespace Logiwa.Application.Queries;

public class SearchProductsQuery : IRequest<List<ProductDto>>
{
    public string? SearchKeyword { get; set; }
    public int? MinStock { get; set; }
    public int? MaxStock { get; set; }

    public SearchProductsQuery(string? searchKeyword, int? minStock, int? maxStock)
    {
        SearchKeyword = searchKeyword;
        MinStock = minStock;
        MaxStock = maxStock;
    }
}

public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public SearchProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products;

        if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
        {
            products = await _productRepository.SearchProducts(request.SearchKeyword, cancellationToken);
        }
        else if (request.MinStock.HasValue || request.MaxStock.HasValue)
        {
            var minStock = request.MinStock ?? 0;
            var maxStock = request.MaxStock ?? int.MaxValue;

            products = await _productRepository.GetProductsByStockRange(minStock, maxStock, cancellationToken);

            return products.Adapt<List<ProductDto>>();
        }

        else
        {
            products = await _productRepository.GetProducts(cancellationToken);
        }

        return products.Adapt<List<ProductDto>>();
    }
}