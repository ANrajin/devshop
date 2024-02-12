using devshop.api.Commons;
using Humanizer;

namespace devshop.api.Features.Books;

public sealed class Book : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public int Price { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public DateOnly PublishedAt { get; set; }

    public string HumanizedPublishedDate()
    {
        var diff = DateTime.UtcNow - PublishedAt.ToDateTime(TimeOnly.MinValue);

        return DateTime.UtcNow.AddDays(- diff.Days).Humanize();
    }
}
