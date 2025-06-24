namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class GetBookCopiesEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBookCopiesGroup()
            .MapGet(string.Empty, async ([FromServices] BooksDbContext booksDb, [FromRoute] Guid id) =>
            {
                ErrorOr<ICollection<BookCopy>> copies = await GetAllBookCopiesAsync(booksDb, id);

                return copies.Match(
                    copies => Results.Ok(copies.ToResponse()),
                    errors => errors.ToProblemResult()
                );
            })
            .Produces<BookCopiesResponse>(StatusCodes.Status200OK)
            .WithDescription("Retrieves all copies of a specific book by its ID")
            .WithName("GetBookCopies")
            .WithSummary("Get All Copies of a Book");
    }

    private static async Task<ErrorOr<ICollection<BookCopy>>> GetAllBookCopiesAsync(BooksDbContext booksDb, Guid bookId)
    {
        Book? book = await booksDb.Books
            .Where(b => b.BookId == bookId)
            .Include(b => b.Copies)
            .FirstOrDefaultAsync();

        if (book is null) return BookErrors.BookNotFound(bookId);

        return book.Copies.ToErrorOr();
    }
}
