using devshop.api.Features.Books.Repositories;

namespace devshop.api.Commons.UnitOfWorks;

public interface IUnitOfWorks
{
    Task SaveAsync(CancellationToken cancellationToken=default);
    
    IBookRepository BookRepository { get; }
}
