namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapGet("{id}", async ([FromRoute] Guid id, [FromServices] BooksDbContext booksDb) =>
            {
                ErrorOr<Book> book = await GetBookAsync(id, booksDb);

                return book.Match(
                    book => Results.Ok(book.ToResponse()),
                    errors => errors.First().Type switch
                    {
                        ErrorType.NotFound => Results.NotFound($"Book with ID {id} not found."),
                        _ => Results.Problem(errors.First().Description)
                    });

            })
            .Produces<BookResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Gets a specific book by its ID from the library catalog")
            .WithName("GetBook")
            .WithSummary("Get a Book by ID");
    }

    private static async Task<ErrorOr<Book>> GetBookAsync(Guid id, BooksDbContext booksDb)
    {
        Book? book = await booksDb.Books
            .Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.BookId == id);

        if (book is null)
        {
            return Error.NotFound();
        }

        return book;
    }
}
