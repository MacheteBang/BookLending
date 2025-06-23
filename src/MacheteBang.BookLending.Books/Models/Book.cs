namespace MacheteBang.BookLending.Books.Models;

public class Book
{
    public Guid BookId { get; private set; }
    public required Isbn Isbn { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }

    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();

    private Book() { }

    public static Book Create(Isbn Isbn, string title, string author)
    {
        return new Book
        {
            BookId = Guid.CreateVersion7(),
            Isbn = Isbn,
            Title = title,
            Author = author
        };
    }
}
