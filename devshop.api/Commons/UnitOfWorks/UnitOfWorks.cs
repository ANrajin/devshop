using devshop.api.Contexts;
using devshop.api.Features.Books.Repositories;

namespace devshop.api.Commons.UnitOfWorks;

public class UnitOfWorks(
    IApplicationDbContext applicationDbContext,
    IBookRepository bookRepository
    )
    : IUnitOfWorks
{
    public async Task SaveAsync(CancellationToken cancellationToken=default) =>
        await applicationDbContext.SaveAsync(cancellationToken);
    
    public IBookRepository BookRepository { get; } = bookRepository;
}
