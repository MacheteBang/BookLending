namespace MacheteBang.BookLending.Books.Errors;

internal static partial class BookErrors
{
    public static Error InvalidIsbn() => Error.Validation(
        code: "Books.InvalidIsbn",
        description: $"The ISBN provided does not conform to ISBN-10 or ISBN-13 standards.");
}
