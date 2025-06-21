namespace MacheteBang.BookLending.Books.Models;

public class Book
{
    public Guid Id { get; private set; }
    public required string Title { get; set; }
    public required string Author { get; set; }

    private Book() { }

    public static Book Create(string title, string author)
    {
        return new Book
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Author = author
        };
    }
}
