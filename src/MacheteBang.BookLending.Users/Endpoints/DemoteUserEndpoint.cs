namespace MacheteBang.BookLending.Users.Endpoints;

internal sealed class DemoteUserEndpoint : IUsersEndpoint
{
    public void MapUsersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{userId}/demote", async ([FromServices] UserManager<User> userManager, Guid userId) =>
            {
                var result = await DemoteUserAsync(userManager, userId);

                return result.Match(
                    success => Results.Accepted(),
                    error => error.ToProblemResult());
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Demotes a user from Administrator role")
            .WithName("DemoteUser")
            .WithSummary("Demote User");
    }

    private static async Task<ErrorOr<Success>> DemoteUserAsync(UserManager<User> userManager, Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return UserErrors.UserNotFound(userId);

        var roleResult = await userManager.RemoveFromRoleAsync(user, Roles.Administrator);
        if (!roleResult.Succeeded)
        {
            // TODO: Look at the various IdentityResult errors and filter where appropriate
            return UserErrors.RoleAssignmentFailed(roleResult.Errors);
        }

        return new Success();
    }
}
