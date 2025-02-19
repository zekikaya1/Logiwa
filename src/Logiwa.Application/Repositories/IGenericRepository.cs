using System.Linq.Expressions;

namespace Application.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default
    );
    
    Task<int> Count(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default
    );

    public Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> filter = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);

    Task<TEntity> GetById(Guid id,
        CancellationToken cancellationToken = default);

    Task Insert(TEntity entity,
        CancellationToken cancellationToken = default);

    void Delete(object id,
        CancellationToken cancellationToken = default);

    void Delete(TEntity entityToDelete);

    void Update(TEntity entityToUpdate);
}