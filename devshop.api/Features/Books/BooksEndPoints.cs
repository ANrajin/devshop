using Bogus;
using devshop.api.Features.Auths.Securities;
using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace devshop.api.Features.Books;

public static class BooksEndPoints
{
    public static RouteGroupBuilder MapBooksApi(this RouteGroupBuilder group)
    {
        GetBooks(group);
        GetBook(group);
        InsertBooks(group);
        UpdateBooks(group);
        DeleteBooks(group);
        InsertSampleBooks(group);

        return group;
    }

    private static void GetBooks(RouteGroupBuilder builder)
    {
        builder.MapGet("/", async Task<Results<Ok<IReadOnlyCollection<BooksResponse>>, ProblemHttpResult>>
            (IBooksService booksService) =>
        {
            try
            {
                var result = await booksService.GetAllBooks();
                return TypedResults.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.Problem();
            }
        })
            .RequireAuthorization()
            .WithName("GetBooks")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.getAll";
                operation.Summary = "Get all books";
                operation.Description = "This endpoint provides a readonly list of all books.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static void GetBook(RouteGroupBuilder builder)
    {
        builder.MapGet("/{id:Guid}", async Task<Results<Ok<BooksResponse>, NotFound, ProblemHttpResult>>
            (Guid id, IBooksService booksService) =>
        {
            try
            {
                var data = await booksService.GetBooks(id);

                return data is not null 
                    ? TypedResults.Ok(data) 
                    : TypedResults.NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.Problem();
            }
        })
            .RequireAuthorization()
            .WithName("GetBook")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.get";
                operation.Summary = "Get specific book item.";
                operation.Description = "This endpoint provides a specific book requested by id.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static void InsertBooks(RouteGroupBuilder builder)
    {
        builder.MapPost("/", async Task<Results<Created, ProblemHttpResult>> 
            (IBooksService booksService, BooksCreateRequest request) =>
        {
            try
            {
                await booksService.InsertBooksAsync(request);
                return TypedResults.Created();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.Problem();
            }
        })
            .RequireAuthorization(DevshopPolicies.BooksPolicy)
            .WithName("InsertBooks")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.create";
                operation.Summary = "Insert book items.";
                operation.Description = "This endpoint creates a specific book item.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static void UpdateBooks(RouteGroupBuilder builder)
    {
        builder.MapPut("/{id:Guid}", async Task<Results<NoContent, ProblemHttpResult>>
            (Guid id, IBooksService booksService, BooksUpdateRequest request) =>
        {
            try
            {
                await booksService.UpdateBooksAsync(id, request);
                return TypedResults.NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.Problem();
            }
        })
            .RequireAuthorization()
            .WithName("UpdateBooks")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.update";
                operation.Summary = "Update book items.";
                operation.Description = "This endpoint update a specific book requested by id.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static void DeleteBooks(RouteGroupBuilder builder)
    {
        builder.MapDelete("/{id:Guid}", async Task<Results<NoContent, NotFound>>
            (Guid id, IBooksService booksService) =>
        {
            try
            {
                await booksService.DestroyBooksAsync(id);
                return TypedResults.NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.NotFound();
            }
        })
            .RequireAuthorization()
            .WithName("DeleteBooks")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.delete";
                operation.Summary = "Delete book items.";
                operation.Description = "This endpoint delete a specific book item requested by id.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static void InsertSampleBooks(RouteGroupBuilder builder)
    {
        builder.MapGet("/sample", async Task<Results<Created, ProblemHttpResult>> (IBooksService service) =>
        {
            try
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

                return TypedResults.Created();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return TypedResults.Problem();
            }
        })
            .RequireAuthorization()
            .WithName("InsertSampleBooks")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "books.sample";
                operation.Summary = "Insert 100 sample book items.";
                operation.Description = "This endpoint insert 100 sample book items for testing.";
                return operation;
            })
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
