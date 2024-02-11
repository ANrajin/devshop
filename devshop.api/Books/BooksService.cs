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
        var books = await applicationDbContext
            .Books
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<IReadOnlyCollection<BooksResponse>>(books);
    }

    public async Task InsertBooksAsync(BooksCreateRequest bookCreate)
    {
        var books = mapper.Map<Book>(bookCreate);
        
        await applicationDbContext.Books.AddAsync(books);
        await applicationDbContext.SaveAsync();
    }

    public async Task UpdateBooksAsync(Guid bookId, BooksUpdateRequest request)
    {
        var book = await applicationDbContext.Books.FindAsync(bookId)
                   ?? throw new ArgumentException("The requested resource not found!");

        mapper.Map(request, book);

        if (request.PublishedAt != DateTime.MinValue)
        {
            book.PublishedAt = DateOnly.FromDateTime(request.PublishedAt);
        }
        
        await applicationDbContext.SaveAsync();
    }

    public async Task DestroyBooksAsync(Guid id)
    {
        var book = await applicationDbContext.Books.FindAsync(id)
                   ?? throw new ArgumentException("The requested item not found!");

        applicationDbContext.Books.Remove(book);
        
        await applicationDbContext.SaveAsync();
    }
}