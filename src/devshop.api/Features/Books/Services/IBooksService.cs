using devshop.api.Features.Books.Requests;

namespace devshop.api.Features.Books.Services;

public interface IBooksService
{
    public Task<IReadOnlyCollection<BooksResponse>> GetAllBooks();
    
    Task<BooksResponse> GetBook(Guid id);

    public Task InsertBooksAsync(BooksCreateRequest bookCreate);

    Task InsertBooksAsync(IEnumerable<Book> books,
        CancellationToken cancellationToken = default);

    public Task UpdateBooksAsync(Guid bookId, BooksUpdateRequest request);
    
    public Task DestroyBooksAsync(Guid id);
}
