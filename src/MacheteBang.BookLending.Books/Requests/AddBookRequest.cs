namespace MacheteBang.BookLending.Books.Requests;

public record AddBookRequest(string Isbn, string Title, string Author);
