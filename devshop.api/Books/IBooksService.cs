namespace devshop.api.Books;

public interface IBooksService
{
    public Task<IReadOnlyCollection<BooksResponse>> GetAllBooks();

    public Task InsertBooksAsync(BooksCreateRequest bookCreate);

    public Task UpdateBooksAsync(Guid bookId, BooksUpdateRequest request);
    
    public Task DestroyBooksAsync(Guid id);
}