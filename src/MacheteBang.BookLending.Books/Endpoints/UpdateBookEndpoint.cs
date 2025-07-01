namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class UpdateBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapPut("{bookId}", async (
                [FromRoute] Guid bookId,
                [FromBody] UpdateBookRequest request,
                [FromServices] BooksDbContext booksDb) =>
            {
                ErrorOr<Book> updatedBookResult = await UpdateBookAsync(bookId, request, booksDb);

                return updatedBookResult.Match(
                    book => Results.Ok(book.ToResponse()),
                    errors => errors.ToProblemResult()
                );
            })
            .RequireAuthorization(Roles.Administrator)
            .Produces<BookResponse>(StatusCodes.Status200OK)
            .WithDescription("Updates a book in the library catalog")
            .WithName("UpdateBook")
            .WithSummary("Update an Existing Book");
    }

    private static async Task<ErrorOr<Book>> UpdateBookAsync(
        Guid bookId,
        UpdateBookRequest request,
        BooksDbContext booksDb)
    {
        // TODO: Capture and handle errors from the database
        Isbn isbn;
        try
        {
            isbn = Isbn.Create(request.Isbn);
        }
        catch (FormatException)
        {
            return BookErrors.InvalidIsbn();
        }

        Book? existingBook = await booksDb.Books
            .Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.BookId == bookId);

        if (existingBook is null)
        {
            return BookErrors.BookNotFound(bookId);
        }

        existingBook.Isbn = isbn;
        existingBook.Title = request.Title;
        existingBook.Author = request.Author;

        await booksDb.SaveChangesAsync();

        return existingBook;
    }
}
