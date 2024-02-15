using AutoMapper;
using devshop.api.Commons.UnitOfWorks;
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

    public async Task<BooksResponse> GetBooks(Guid id)
    {
        var book = await unitOfWorks.BookRepository.GetByIdAsync(id)
                   ?? throw new ArgumentException("The requested resource not found!");

        return mapper.Map<BooksResponse>(book);
    }

    public async Task InsertBooksAsync(BooksCreateRequest bookCreate)
    {
        var books = mapper.Map<Book>(bookCreate);

        await unitOfWorks.BookRepository.InsertAsync(books);
        await unitOfWorks.SaveAsync();
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
