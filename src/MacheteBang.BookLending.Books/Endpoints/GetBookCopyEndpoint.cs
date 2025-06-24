namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBookCopyEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBookCopiesGroup()
            .MapGet("{copyId}", async ([FromRoute] Guid id, [FromRoute] Guid copyId, [FromServices] BooksDbContext booksDb) =>
            {
                ErrorOr<BookCopy> bookCopy = await GetBookCopyAsync(id, copyId, booksDb);

                return bookCopy.Match(
                    bookCopy => Results.Ok(bookCopy.ToResponse()),
                    errors => errors.ToProblemResult());
            })
            .Produces<BookCopyResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Gets a specific copy of a book by its ID")
            .WithName("GetBookCopy")
            .WithSummary("Get a Book Copy by ID");
    }

    private static async Task<ErrorOr<BookCopy>> GetBookCopyAsync(Guid bookId, Guid copyId, BooksDbContext booksDb)
    {
        BookCopy? bookCopy = await booksDb.BookCopies
            .FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.BookCopyId == copyId);

        if (bookCopy is null) return BookErrors.CopyNotFound(copyId);

        return bookCopy;
    }
}
