using devshop.api.Auths.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) 
    : IdentityDbContext<ApplicationUser, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
        (dbContextOptions), IApplicationDbContext
{
    public async Task SaveAsync(CancellationToken cancellationToken) 
        => await SaveChangesAsync(cancellationToken);

    public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        => Set<TEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Set Table Name as devshop.TableName
        modelBuilder.HasDefaultSchema("devshop");
            
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
