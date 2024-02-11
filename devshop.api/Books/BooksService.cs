using AutoMapper;
using devshop.api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Books;

public sealed class BooksService(
    IApplicationDbContext applicationDbContext,
    IMapper mapper) 
    : IBooksService
{
    public async Task<IReadOnlyCollection<BooksResponse>> GetAllBooks()
    {
        var books = await applicationDbContext.Books.ToListAsync();

        return mapper.Map<IReadOnlyCollection<BooksResponse>>(books);
    }

    public async Task InsertBooks(Book book)
    {
        await applicationDbContext.Books.AddAsync(book);
        await applicationDbContext.SaveAsync();
    }
}