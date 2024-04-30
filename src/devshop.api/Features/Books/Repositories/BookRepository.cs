using devshop.api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Features.Books.Repositories;

public class BookRepository(IApplicationDbContext dbContext) : IBookRepository
{
    private readonly DbSet<Book> _dbSet = dbContext.DbSet<Book>();
    
    public async Task<IReadOnlyCollection<Book>> GetAllAsync(
        bool shouldTrack=false, 
        CancellationToken cancellationToken=default)
    {
        if (shouldTrack)
            return await _dbSet.TagWith("Select All Books").AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
        
        return await _dbSet.TagWith("Select All Books")
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(Book book,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(book);
    }

    public async Task InsertRangeAsync(IEnumerable<Book> books, 
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(books, cancellationToken);
    }

    public async Task DeleteAsync(Book book, 
        CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            if (dbContext.Entry(book).State == EntityState.Detached)
            {
                _dbSet.Attach(book);
            }
            _dbSet.Remove(book);
        });
    }
}