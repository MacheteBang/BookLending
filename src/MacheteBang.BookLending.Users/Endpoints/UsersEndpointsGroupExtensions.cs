namespace MacheteBang.BookLending.Users.Endpoints;

internal static class UserEndpointsGroupExtensions
{
    internal static IEndpointRouteBuilder MapUsersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("/users")
            .WithTags("Users");
    }
}
