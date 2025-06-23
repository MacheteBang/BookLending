namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class AddBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapPost(string.Empty, async ([FromBody] AddBookRequest request, [FromServices] BooksDbContext booksDb) =>
            {
                var newBookResult = await CreateBook(request, booksDb);
                if (newBookResult.IsError && newBookResult.Errors.First().Type == ErrorType.Validation)
                {
                    // TODO: Add this to global exception handling
                    var errorDictionary = newBookResult.Errors
                        .GroupBy(e => e.Code ?? "General")
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.Description).ToArray()
                        );
                    return Results.ValidationProblem(errorDictionary);
                }
                if (newBookResult.IsError) return Results.Problem("An unexpected error occurred.");

                Book newBook = newBookResult.Value;

                return Results.CreatedAtRoute("GetBook", new { id = newBook.BookId }, newBook.ToResponse());

            })
            .Produces<BookResponse>(StatusCodes.Status201Created)
            .WithDescription("Adds a new book to the library catalog")
            .WithName("AddBook")
            .WithSummary("Add a New Book");
    }

    private static async Task<ErrorOr<Book>> CreateBook(AddBookRequest request, BooksDbContext booksDb)
    {
        Isbn isbn;
        try
        {
            isbn = Isbn.Create(request.Isbn);
        }
        catch
        {
            return Error.Validation("Invalid ISBN format.");
        }

        Book newBook = Book.Create(isbn, request.Title, request.Author);
        await booksDb.Books.AddAsync(newBook);
        await booksDb.SaveChangesAsync();
        return newBook;
    }
}
