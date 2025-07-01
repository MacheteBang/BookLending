namespace MacheteBang.BookLending.Users.Endpoints;

internal sealed class PromoteUserEndpoint : IUsersEndpoint
{
    public void MapUsersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{userId}/promote", async ([FromServices] UserManager<User> userManager, Guid userId) =>
            {
                var result = await PromoteUserAsync(userManager, userId);

                return result.Match(
                    success => Results.Accepted(),
                    error => error.ToProblemResult());
            })
            .RequireAuthorization(Roles.Administrator)
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Promotes a user to Administrator")
            .WithName("PromoteUser")
            .WithSummary("Promote User");
    }

    private static async Task<ErrorOr<Success>> PromoteUserAsync(UserManager<User> userManager, Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null) return UserErrors.UserNotFound(userId);

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Administrator);
        if (!roleResult.Succeeded)
        {
            // TODO: Look at the various IdentityResult errors and filter where appropriate
            return UserErrors.RoleAssignmentFailed(roleResult.Errors);
        }

        return new Success();
    }
}
