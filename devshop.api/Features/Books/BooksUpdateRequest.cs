namespace devshop.api.Features.Books;

public sealed record BooksUpdateRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);
