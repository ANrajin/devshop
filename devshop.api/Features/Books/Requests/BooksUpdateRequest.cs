namespace devshop.api.Features.Books.Requests;

public sealed record BooksUpdateRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);
