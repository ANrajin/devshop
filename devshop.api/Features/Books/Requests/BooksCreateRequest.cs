namespace devshop.api.Features.Books.Requests;

public sealed record BooksCreateRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);
