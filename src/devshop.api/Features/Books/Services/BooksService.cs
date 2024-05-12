using AutoMapper;
using devshop.api.Cores.UnitOfWorks;
using devshop.api.Features.Books.Requests;

namespace devshop.api.Features.Books.Services;

public sealed class BooksService(
    IUnitOfWorks unitOfWorks,
    IMapper mapper)
    : IBooksService
{
    public async Task<IReadOnlyCollection<BooksResponse>> GetAllBooks()
    {
        var books = await unitOfWorks.BookRepository.GetAllAsync();

        return mapper.Map<IReadOnlyCollection<BooksResponse>>(books);
    }

    public async Task<BooksResponse> GetBook(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("The book id is invalid.");
        }

        var book = await unitOfWorks.BookRepository.GetByIdAsync(id)
                   ?? throw new ArgumentException("The requested resource was not found!");

        return mapper.Map<BooksResponse>(book);
    }

    public async Task InsertBooksAsync(BooksCreateRequest bookCreate)
    {
        ArgumentNullException.ThrowIfNull(bookCreate);

        var books = mapper.Map<Book>(bookCreate);

        await unitOfWorks.BookRepository.InsertAsync(books);
        await unitOfWorks.SaveAsync();
    }

    public async Task InsertBooksAsync(IEnumerable<Book> books, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(books);

        if(!books.Any())
        {
            throw new ArgumentException("The books list cannot be empty.");
        }

        await unitOfWorks.BookRepository.InsertRangeAsync(books, cancellationToken);
        await unitOfWorks.SaveAsync(cancellationToken);
    }

    public async Task UpdateBooksAsync(Guid id, BooksUpdateRequest request)
    {
        var book = await unitOfWorks.BookRepository.GetByIdAsync(id)
                   ?? throw new ArgumentException("The requested resource not found!");

        mapper.Map(request, book);

        if (request.PublishedAt != DateTime.MinValue)
        {
            book.PublishedAt = DateOnly.FromDateTime(request.PublishedAt);
        }
        
        await unitOfWorks.SaveAsync();
    }

    public async Task DestroyBooksAsync(Guid id)
    {
        var book = await unitOfWorks.BookRepository.GetByIdAsync(id)
                   ?? throw new ArgumentException("The requested item not found!");

        await unitOfWorks.BookRepository.DeleteAsync(book);
        
        await unitOfWorks.SaveAsync();
    }
}
