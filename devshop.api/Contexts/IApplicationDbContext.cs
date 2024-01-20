using devshop.api.Books;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Contexts;

public interface IApplicationDbContext
{
    public Task SaveAsync(CancellationToken cancellationToken = default);
    
    public DbSet<Book> Books { get; }
}