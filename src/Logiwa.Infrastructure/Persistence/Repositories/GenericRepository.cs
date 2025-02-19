using System.Linq.Expressions;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Logiwa.Infrastructure.Persistence.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected GenericRepository(LogiwaDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    protected LogiwaDbContext Context { get; init; }
    protected DbSet<TEntity> DbSet { get; init; }

    public Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                 (new[]
                 {
                     ','
                 }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null) return orderBy(query).ToListAsync(cancellationToken);

        return query.ToListAsync(cancellationToken);
    }
    
    public Task<int> Count(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                 (new[]
                 {
                     ','
                 }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null) return orderBy(query).CountAsync(cancellationToken);

        return query.CountAsync(cancellationToken);
    }

    public async Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> filter = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                 (new[]
                 {
                     ','
                 }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetById(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task Insert(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async void Delete(object id, CancellationToken cancellationToken)
    {
        var entityToDelete = await DbSet.FindAsync(id, cancellationToken);
        Delete(entityToDelete);
    }


    public virtual void Delete(TEntity entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached) DbSet.Attach(entityToDelete);

        DbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}