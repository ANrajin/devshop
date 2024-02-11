using AutoMapper;

namespace devshop.api.Books;

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
        });

        app.MapPost("/books", async (
            IBooksService booksService,
            IMapper mapper,
            BooksRequest request) =>
        {
            try
            {
                var books = mapper.Map<Book>(request);
                await booksService.InsertBooks(books);
                return Results.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.Problem();
            }
        });
    }
}