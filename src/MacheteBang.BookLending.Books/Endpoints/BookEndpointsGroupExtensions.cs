namespace MacheteBang.BookLending.Books.Endpoints;

internal static class BookEndpointsGroupExtensions
{
    internal static IEndpointRouteBuilder MapBooksGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("/books")
            .WithTags("Books");
    }

    internal static IEndpointRouteBuilder MapBookCopiesGroup(this IEndpointRouteBuilder booksGroup)
    {
        return booksGroup
            .MapBooksGroup()
            .MapGroup("/{id}/copies")
            .WithTags("BookCopies");
    }
}
