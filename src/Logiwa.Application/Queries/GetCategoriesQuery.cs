using Logiwa.Application.Models.Category;
using Logiwa.Application.Repositories;
using Mapster;
using MediatR;

namespace Logiwa.Application.Queries;

public class GetCategoriesQuery : IRequest<List<CategoryDto>> { }

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Adapt<List<CategoryDto>>();
    }
}