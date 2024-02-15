using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace devshop.api.Contexts;

public interface IApplicationDbContext
{
    public Task SaveAsync(CancellationToken cancellationToken = default);
    
    DbSet<TEntity> DbSet<TEntity>() where TEntity : class;

    EntityEntry Entry(object entity);
}
