namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBooksEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapGet(string.Empty, async ([FromServices] BooksDbContext booksDb, [FromQuery] string? q) =>
            {
                ICollection<Book> books = await GetAllBooksAsync(booksDb, q);

                return Results.Ok(books.ToResponse());
            })
            .Produces<BooksResponse>(StatusCodes.Status200OK)
            .WithDescription("Retrieves the complete list of books in the library catalog")
            .WithName("GetBooks")
            .WithSummary("Get All Books");
    }

    private static async Task<ICollection<Book>> GetAllBooksAsync(BooksDbContext booksDb, string? query = null)
    {
        var booksQuery = booksDb.Books
            .Include(b => b.Copies)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(query) || b.Author.Contains(query));
        }

        // TODO: Should this be a better return type?
        return await booksQuery.ToArrayAsync();
    }
}
