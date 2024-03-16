using Bogus;
using devshop.api.Features.Auths.Securities;
using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;

namespace devshop.api.Features.Books;

public static class BooksEndPoints
{
    private const string _tag = "Book";

    public static void MapBookEndPoints(this IEndpointRouteBuilder app)
    {
        GetBooks(app);
        GetBook(app);
        InsertBooks(app);
        UpdateBooks(app);
        DeleteBooks(app);
        InsertSampleBooks(app);
    }

    private static void GetBooks(IEndpointRouteBuilder app)
    {
        app.MapGet("/books", async (IBooksService booksService) =>
        {
            try
            {
                var result = await booksService.GetAllBooks();

                return TypedResults.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem();
            }
        })
            .WithTags(_tag)
            .WithName("GetBooks")
            .RequireAuthorization()
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books";
                operation.Summary = "Get all books";
                operation.Description = "This endpoint provides a readonly list of all books.";
                return operation;
            })
            .Produces<IReadOnlyCollection<Book>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static void GetBook(IEndpointRouteBuilder app)
    {
        app.MapGet("books/{id:Guid}", async (Guid id, IBooksService booksService) =>
        {
            try
            {
                var data = await booksService.GetBooks(id);
                return Results.Ok(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.NotFound();
            }
        })
            .WithTags(_tag)
            .RequireAuthorization();
    }

    private static void InsertBooks(IEndpointRouteBuilder app)
    {
        app.MapPost("/books", async (
        IBooksService booksService,
        BooksCreateRequest request) =>
        {
            try
            {
                await booksService.InsertBooksAsync(request);
                return Results.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem();
            }
        })
            .WithTags(_tag)
            .RequireAuthorization(DevshopPolicies.BooksPolicy);
    }

    private static void UpdateBooks(IEndpointRouteBuilder app)
    {
        app.MapPut("/books/{id:Guid}", async (
        Guid id,
        IBooksService booksService,
        BooksUpdateRequest request) =>
        {
            try
            {
                await booksService.UpdateBooksAsync(id, request);
                return Results.NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem();
            }
        })
            .WithTags(_tag)
            .RequireAuthorization();
    }

    private static void DeleteBooks(IEndpointRouteBuilder app)
    {
        app.MapDelete("/books/{id:Guid}", async (
        Guid id,
        IBooksService booksService) =>
        {
            try
            {
                await booksService.DestroyBooksAsync(id);
                return Results.NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.NotFound();
            }
        })
            .WithTags(_tag)
            .RequireAuthorization();
    }

    private static void InsertSampleBooks(IEndpointRouteBuilder app)
    {
        app.MapGet("/sample", async (IBooksService service) =>
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
            .WithTags(_tag)
            .RequireAuthorization();
    }
}
