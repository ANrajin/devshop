namespace devshop.api.Books;

public interface IBooksService
{
    public Task<IReadOnlyCollection<Book>> GetAllBooks();

    public Task InsertBooks(Book book);
}