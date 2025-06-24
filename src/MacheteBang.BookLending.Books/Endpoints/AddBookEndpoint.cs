namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class AddBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapPost(string.Empty, async ([FromBody] AddBookRequest request, [FromServices] BooksDbContext booksDb) =>
            {
                ErrorOr<Book> newBookResult = await AddBookAsync(request, booksDb);

                return newBookResult.Match(
                    book => Results.CreatedAtRoute("GetBook", new { id = book.BookId }, book.ToResponse()),
                    errors => errors.ToProblemResult()
                );
            })
            .Produces<BookResponse>(StatusCodes.Status201Created)
            .WithDescription("Adds a new book to the library catalog")
            .WithName("AddBook")
            .WithSummary("Add a New Book");
    }

    private static async Task<ErrorOr<Book>> AddBookAsync(AddBookRequest request, BooksDbContext booksDb)
    {
        Isbn isbn;
        try
        {
            isbn = Isbn.Create(request.Isbn);
        }
        catch (FormatException)
        {
            return BookErrors.InvalidIsbn();
        }

        Book newBook = Book.Create(isbn, request.Title, request.Author);
        await booksDb.Books.AddAsync(newBook);
        await booksDb.SaveChangesAsync();
        return newBook;
    }
}
