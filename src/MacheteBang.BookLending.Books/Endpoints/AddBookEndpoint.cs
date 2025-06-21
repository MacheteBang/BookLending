namespace MacheteBang.BookLending.Books.Endpoints;

internal class AddBookEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapPost(string.Empty, async ([FromBody] AddBookRequest request, [FromServices] BooksDbContext booksDb) =>
            {
                Book newBook = await CreateBook(request, booksDb);

                return Results.CreatedAtRoute("GetBook", new { id = newBook.Id }, newBook.ToResponse());

            })
            .WithDescription("Adds a new book to the library catalog")
            .WithName("AddBook")
            .WithSummary("Add a New Book");
    }

    private static async Task<Book> CreateBook(AddBookRequest request, BooksDbContext booksDb)
    {
        Book newBook = Book.Create(request.Title, request.Author);
        await booksDb.Books.AddAsync(newBook);
        await booksDb.SaveChangesAsync();
        return newBook;
    }
}
