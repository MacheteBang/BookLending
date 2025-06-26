namespace MacheteBang.BookLending.Users.Endpoints;

internal sealed class GetUsersEndpoint : IUsersEndpoint
{
    public void MapUsersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapGet(string.Empty, async ([FromServices] UserManager<User> userManager) =>
            {
                var users = await GetAllUsersAsync(userManager);
                return Results.Ok(users.ToResponse());
            })
            .Produces<UsersResponse>(StatusCodes.Status200OK)
            .WithDescription("Gets the complete list of users in the system")
            .WithName("GetAllUsers")
            .WithSummary("Get All Users");
    }

    private static async Task<ICollection<User>> GetAllUsersAsync(UserManager<User> userManager)
    {
        return await userManager.Users.ToArrayAsync();
    }
}
