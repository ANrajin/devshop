using Bogus;
using devshop.api.Features.Books;
using devshop.api.Features.Books.Services;

namespace devshop.api.Features.Samples;

public static class SamplesEndPoint
{
    public static void MapSamplesEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/books", async (IBooksService service) =>
            {
                var faker = new Faker<Book>();
                faker.RuleFor(b => b.Name, f => f.Commerce.ProductName());
                faker.RuleFor(b => b.Description, f => f.Commerce.ProductDescription());
                faker.RuleFor(b => b.Price, f => f.Random.Int(100, 5000));

                var startDate = DateOnly.FromDateTime(new DateTime(1920, 02, 01));
                var endDate = DateOnly.FromDateTime(new DateTime(2020, 12, 30));
                faker.RuleFor(b => b.PublishedAt, f => f.Date.BetweenDateOnly(startDate, endDate));

                var books = faker.Generate(100);

                await service.InsertBooksAsync(books);

                return Results.Ok();
            })
            .WithTags("sample");
    }
}