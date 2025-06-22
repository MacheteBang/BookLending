namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class RemoveBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapDelete("{id}", async ([FromRoute] Guid id, [FromServices] BooksDbContext booksDb) =>
            {
                await RemoveBookAsync(id, booksDb);
                return Results.Accepted();
            })
            .Produces(StatusCodes.Status202Accepted)
            .WithDescription("Removes a specific book by its ID from the library catalog")
            .WithName("RemoveBook")
            .WithSummary("Remove a Book by ID");
    }

    private static async Task RemoveBookAsync(Guid id, BooksDbContext booksDb)
    {
        await booksDb.Books
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
    }
}
