using devshop.api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Books;

public sealed class BooksService(IApplicationDbContext applicationDbContext) 
    : IBooksService
{
    public async Task<IReadOnlyCollection<Book>> GetAllBooks()
    {
        return await applicationDbContext.Books.ToListAsync();
    }

    public async Task InsertBooks(Book book)
    {
        await applicationDbContext.Books.AddAsync(book);
        await applicationDbContext.SaveAsync();
    }
}