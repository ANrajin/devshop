namespace devshop.api.Books;

public sealed record BooksRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);