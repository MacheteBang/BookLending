namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBooksEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapGet(string.Empty, async ([FromServices] BooksDbContext booksDb) =>
            {
                List<Book> books = await GetAllBooksAsync(booksDb);

                return Results.Ok(books.ToResponse());
            })
            .Produces<BooksResponse>(StatusCodes.Status200OK)
            .WithDescription("Retrieves the complete list of books in the library catalog")
            .WithName("GetBooks")
            .WithSummary("Get All Books");
    }

    private static async Task<List<Book>> GetAllBooksAsync(BooksDbContext booksDb)
    {
        return await booksDb.Books
            .Include(b => b.Copies)
            .ToListAsync();
    }
}
