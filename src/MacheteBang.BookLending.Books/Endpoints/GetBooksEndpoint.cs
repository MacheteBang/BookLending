using MacheteBang.BookLending.Books.Models;

namespace MacheteBang.BookLending.Books.Endpoints;

internal class GetBooksEndpoint : IBooksEndpoint
{
    public void MapBooksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapBooksGroup()
            .MapGet(string.Empty, () =>
            {
                Book[] books =
                [
                    new Book(Guid.CreateVersion7(), "The Hitchhiker's Guide to the Galaxy", "Douglas Adams"),
                    new Book(Guid.CreateVersion7(), "1984", "George Orwell"),
                    new Book(Guid.CreateVersion7(), "To Kill a Mockingbird", "Harper Lee")
                ];

                return Results.Ok(books.Select(book => book.ToResponse()).ToList());
            })
            .WithDescription("Retrieves the complete list of books in the library catalog")
            .WithName("GetBooks")
            .WithSummary("Get All Books");
    }
}
