using devshop.api.Features.Books.Services;

namespace devshop.api.Features.Books.Requests;

public sealed class BooksRequestHandler(IBooksService booksService)
{
    public async Task<IReadOnlyCollection<BooksResponse>> Books()
    {
        return await booksService.GetAllBooks();
    }
    
    public async Task<BooksResponse> Books(Guid id)
    {
        return await booksService.GetBook(id);
    }
    
    public async Task InsertAsync(BooksCreateRequest request)
    {
        await booksService.InsertBooksAsync(request);
    }

    public async Task UpdateAsync(Guid id, BooksUpdateRequest request)
    {
        await booksService.UpdateBooksAsync(id, request);
    }

    public async Task DeleteAsync(Guid id)
    {
        await booksService.DestroyBooksAsync(id);
    }
}