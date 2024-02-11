namespace devshop.api.Books;

public interface IBooksService
{
    public Task<IReadOnlyCollection<BooksResponse>> GetAllBooks();

    public Task InsertBooks(Book book);
}