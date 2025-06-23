namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBookCopiesEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBookCopiesGroup()
            .MapGet(string.Empty, async ([FromServices] BooksDbContext booksDb, [FromRoute] Guid id) =>
            {
                List<BookCopy> copies = await GetAllBookCopiesAsync(booksDb, id);

                return Results.Ok(copies.ToResponse());
            })
            .Produces<BookCopiesResponse>(StatusCodes.Status200OK)
            .WithDescription("Retrieves all copies of a specific book by its ID")
            .WithName("GetBookCopies")
            .WithSummary("Get All Copies of a Book");
    }

    private static async Task<List<BookCopy>> GetAllBookCopiesAsync(BooksDbContext booksDb, Guid bookId)
    {
        // TODO: Check if the book exists, return 404 if not found
        return await booksDb.BookCopies
            .Where(bc => bc.BookId == bookId)
            .ToListAsync();
    }
}
