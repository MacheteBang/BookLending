namespace MacheteBang.BookLending.Books.Errors;

internal static partial class BookErrors
{
    public static Error CopyNotFound(Guid bookCopyId) => Error.NotFound(
        code: "Books.CopyNotFound",
        description: $"The specified copy with id of {bookCopyId} was not found.");
}
