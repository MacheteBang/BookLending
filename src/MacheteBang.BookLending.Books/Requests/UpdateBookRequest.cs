namespace MacheteBang.BookLending.Books.Requests;

public record UpdateBookRequest(string Isbn, string Title, string Author);
