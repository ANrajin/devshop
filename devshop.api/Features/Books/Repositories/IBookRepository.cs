namespace devshop.api.Features.Books.Repositories;

public interface IBookRepository
{
    Task<IReadOnlyCollection<Book>> GetAllAsync(bool shouldTrack=false, 
        CancellationToken cancellationToken=default);

    Task<Book?> GetByIdAsync(Guid id);

    Task InsertAsync(Book book, 
        CancellationToken cancellationToken = default);

    Task InsertRangeAsync(IEnumerable<Book> books,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Book book, 
        CancellationToken cancellationToken = default);
}