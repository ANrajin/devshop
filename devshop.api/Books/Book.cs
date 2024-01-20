using devshop.api.Commons;

namespace devshop.api.Books;

public sealed class Book : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public int Price { get; set; }

    public string Description { get; set; } = string.Empty;
}