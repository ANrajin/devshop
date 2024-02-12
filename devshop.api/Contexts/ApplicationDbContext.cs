using devshop.api.Auths.Entities;
using devshop.api.Features.Books;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) 
    : IdentityDbContext<ApplicationUser, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
        (dbContextOptions), IApplicationDbContext
{
    public async Task SaveAsync(CancellationToken cancellationToken) 
        => await SaveChangesAsync(cancellationToken);

    public DbSet<Book> Books => Set<Book>();
}
