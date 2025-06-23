namespace MacheteBang.BookLending.Books.Responses;

public record BookCopyResponse(Guid BookCopyId, Guid BookId, string Condition);
public record BookCopiesResponse(int Count, List<BookCopyResponse> Copies);

public static class BookCopyResponseExtensions
{
    public static BookCopyResponse ToResponse(this BookCopy copy)
    {
        return new BookCopyResponse(
            copy.BookCopyId,
            copy.BookId,
            copy.Condition);
    }

    public static BookCopiesResponse ToResponse(this ICollection<BookCopy> copies)
    {
        return new BookCopiesResponse(
            copies.Count,
            copies
                .Select(c => c.ToResponse())
                .ToList());
    }
}
