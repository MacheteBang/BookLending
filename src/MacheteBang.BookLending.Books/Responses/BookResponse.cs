namespace MacheteBang.BookLending.Books.Responses;

public record BookResponse(Guid Id, string Title, string Author);

public static class BookResponseExtensions
{
    public static BookResponse ToResponse(this Models.Book book)
    {
        return new BookResponse(book.Id, book.Title, book.Author);
    }
}
