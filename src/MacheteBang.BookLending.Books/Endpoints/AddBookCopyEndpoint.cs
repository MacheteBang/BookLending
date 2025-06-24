namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class AddBookCopyEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBookCopiesGroup().MapPost(string.Empty, async ([FromRoute] Guid id, [FromBody] AddBookCopyRequest request, [FromServices] BooksDbContext booksDb) =>
            {
                ErrorOr<BookCopy> result = await AddBookCopyAsync(id, request, booksDb);

                return result.Match(
                    bookCopy => Results.CreatedAtRoute("GetBookCopy", new { id = bookCopy.BookId, copyId = bookCopy.BookCopyId }, bookCopy.ToResponse()),
                    errors => errors.ToProblemResult());
            })
            .Produces<BookCopyResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Adds a new copy of a book to the library catalog")
            .WithName("AddBookCopy")
            .WithSummary("Add a New Book Copy");
    }
    private static async Task<ErrorOr<BookCopy>> AddBookCopyAsync(Guid bookId, AddBookCopyRequest request, BooksDbContext booksDb)
    {
        Book? book = await booksDb.Books
            .FirstOrDefaultAsync(b => b.BookId == bookId);

        if (book is null) return BookErrors.BookNotFound(bookId);

        BookCopy newCopy = BookCopy.Create(book, request.Condition);
        await booksDb.BookCopies.AddAsync(newCopy);
        await booksDb.SaveChangesAsync();
        return newCopy;
    }
}
