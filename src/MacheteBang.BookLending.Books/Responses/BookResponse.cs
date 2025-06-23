namespace MacheteBang.BookLending.Books.Responses;

public record BookResponse(Guid Id, string Isbn, string Title, string Author);
public record BooksResponse(int Count, List<BookResponse> Books);

public static class BookResponseExtensions
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(book.BookId, book.Isbn.Value, book.Title, book.Author);
    }

    public static BooksResponse ToResponse(this ICollection<Book> books)
    {
        return new BooksResponse(
            books.Count,
            books.Select(b => b.ToResponse()).ToList());
    }
}
