namespace MacheteBang.BookLending.Books.Models;

public class BookCopy
{
    public Guid BookCopyId { get; private set; }
    public Guid BookId { get; private set; }
    public required string Condition { get; set; }

    public Book Book { get; set; } = null!;

    private BookCopy() { }

    public static BookCopy Create(Book book, string condition)
    {
        return new BookCopy
        {
            BookCopyId = Guid.CreateVersion7(),
            BookId = book.BookId,
            Condition = condition
        };
    }
}
