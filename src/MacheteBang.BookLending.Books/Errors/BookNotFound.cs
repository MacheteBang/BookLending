namespace MacheteBang.BookLending.Books.Errors;

internal static partial class BookErrors
{
    public static Error BookNotFound(Guid bookId) => Error.NotFound(
        code: "Books.BookNotFound",
        description: $"The specified book with id of {bookId} was not found.");
}
