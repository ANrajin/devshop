using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;

namespace devshop.api.Features.Books;

public static class BooksEndPoints
{
    public static void MapBookEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/books", async (IBooksService booksService) =>
        {
            try
            {
                var result = await booksService.GetAllBooks();
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem();
            }
        }).WithTags("Books");

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
        }).WithTags("Books");

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
        .WithTags("Books")
        .RequireAuthorization();

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
        .WithTags("Books");
        
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
        .WithTags("Books");
    }
}
