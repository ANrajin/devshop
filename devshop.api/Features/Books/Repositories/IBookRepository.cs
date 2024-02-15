namespace devshop.api.Features.Books.Repositories;

public interface IBookRepository
{
    Task<IReadOnlyCollection<Book>> GetAllAsync(bool shouldTrack=false, 
        CancellationToken cancellationToken=default);

    Task<Book?> GetByIdAsync(Guid id);

    Task InsertAsync(Book book);

    Task DeleteAsync(Book book);
}