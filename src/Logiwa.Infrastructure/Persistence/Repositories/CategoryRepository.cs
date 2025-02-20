using Logiwa.Application.Repositories;
using Logiwa.Core.Entities; 
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LogiwaDbContext _context;

    public CategoryRepository(LogiwaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}