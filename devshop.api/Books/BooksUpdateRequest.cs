namespace devshop.api.Books;

public sealed record BooksUpdateRequest(
    string Name,
    int Price,
    string Description,
    DateTime PublishedAt);