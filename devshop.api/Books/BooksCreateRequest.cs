namespace devshop.api.Books;

public sealed record BooksCreateRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);