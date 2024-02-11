namespace devshop.api.Books;

public sealed class BooksResponse
{
    public string Name { get; set; } = string.Empty;
    
    public int Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public string PublishedAt { get; set; } = string.Empty;
}