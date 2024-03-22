using devshop.api.Features.Books;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace devshop.api.Contexts.Configs;

public sealed class BooksConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable(nameof(Book).Pluralize());
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(255);

        builder.Property(x => x.Price)
            .HasDefaultValue(0);
    }
}