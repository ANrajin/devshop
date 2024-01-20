using devshop.api.Books;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : DbContext(dbContextOptions), IApplicationDbContext
{
    public async Task SaveAsync(CancellationToken cancellationToken) 
        => await SaveChangesAsync(cancellationToken);

    public DbSet<Book> Books => Set<Book>();
}