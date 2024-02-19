using System.Linq.Expressions;

namespace devshop.api.Cores.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<IEnumerable<TEntity>> FindAsync(CancellationToken cancellationToken=default);

    Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken=default);

    Task<TEntity?> FindOnlyOrFail(Expression<Func<TEntity, bool>> query,
        CancellationToken cancellationToken=default);

    Task<TEntity?> FindFirstOrFail(Expression<Func<TEntity, bool>> query,
        CancellationToken cancellationToken=default);
    
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken=default);

    Task CreateBatchAsync(IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken=default);

    Task RemoveAsync(TEntity entity);
}