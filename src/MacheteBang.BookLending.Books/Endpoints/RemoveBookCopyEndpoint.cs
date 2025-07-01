namespace MacheteBang.BookLending.Books.Endpoints;

internal sealed class RemoveBookCopyEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBookCopiesGroup()
            .MapDelete("{copyId}", async ([FromRoute] Guid id, [FromRoute] Guid copyId, [FromServices] BooksDbContext booksDb) =>
            {
                await RemoveBookCopyAsync(id, copyId, booksDb);

                return Results.Accepted();
            })
            .RequireAuthorization(Roles.Administrator)
            .Produces(StatusCodes.Status202Accepted)
            .WithDescription("Removes a specific copy of a book from the library catalog")
            .WithName("RemoveBookCopy")
            .WithSummary("Remove a Book Copy by ID");
    }

    private static async Task<Success> RemoveBookCopyAsync(Guid bookId, Guid copyId, BooksDbContext booksDb)
    {
        int deleted = await booksDb.BookCopies
            .Where(bc => bc.BookId == bookId && bc.BookCopyId == copyId)
            .ExecuteDeleteAsync();

        return Result.Success;
    }
}
