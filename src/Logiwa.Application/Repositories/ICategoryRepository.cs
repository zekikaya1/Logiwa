using Logiwa.Core.Entities;

namespace Logiwa.Application.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
}