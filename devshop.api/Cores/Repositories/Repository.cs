using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Cores.Repositories;

public abstract class Repository<TEntity, TKey>(DbContext dbContext) 
    : IRepository<TEntity, TKey>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken=default)
    {
        return await _dbSet.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken=default)
    {
        return await _dbSet.FindAsync([id, cancellationToken], 
            cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> FindOnlyOrFail(Expression<Func<TEntity, bool>> query, 
        CancellationToken cancellationToken=default)
    {
        return await _dbSet.SingleOrDefaultAsync(query, cancellationToken);
    }

    public async Task<TEntity?> FindFirstOrFail(Expression<Func<TEntity, bool>> query,
        CancellationToken cancellationToken=default)
    {
        return await _dbSet.FirstOrDefaultAsync(query, cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, 
        CancellationToken cancellationToken=default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task CreateBatchAsync(IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken=default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }
    
    public async Task RemoveAsync(TEntity entity)
    {
        await Task.Run(() =>
        {
            _dbSet.Remove(entity);
        });
    }
}