using devshop.api.Features.Books.Repositories;

namespace devshop.api.Cores.UnitOfWorks;

public interface IUnitOfWorks
{
    Task SaveAsync(CancellationToken cancellationToken=default);
    
    IBookRepository BookRepository { get; }
}
