namespace devshop.api.Books;

public static class BookEndPoints
{
    public static void MapBookEndPoints
        (this IEndpointRouteBuilder app)
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

        app.MapPost("/books", async (IBooksService booksService, Book book) =>
        {
            try
            {
                await booksService.InsertBooks(book);
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