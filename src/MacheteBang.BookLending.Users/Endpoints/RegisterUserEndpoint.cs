namespace MacheteBang.BookLending.Users.Endpoints;

internal sealed class RegisterUserEndpoint : IUsersEndpoint
{
    public void MapUsersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPost("/register", async ([FromServices] UserManager<User> userManager, RegisterUserRequest request) =>
            {
                if (!request.Email.IsValidEmail()) return KernelErrors.InvalidEmail(request.Email).ToProblemResult();

                var result = await RegisterUserAsync(userManager, request);
                return result.Match(
                    user => Results.Created($"/users/{user.Id}", user.ToResponse()),
                    error => error.ToProblemResult());
            })
            .Produces<UserResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Registers a new user")
            .WithName("RegisterUser")
            .WithSummary("Register User");
    }

    private static async Task<ErrorOr<User>> RegisterUserAsync(UserManager<User> userManager, RegisterUserRequest request)
    {
        User user = new() { UserName = request.Email, Email = request.Email };

        IdentityResult result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code.StartsWith("Invalid")))
            {
                return UserErrors.InvalidUserName(request.Email);
            }

            if (result.Errors.Any(e => e.Code.StartsWith("Password")))
            {
                return UserErrors.InvalidPassword(result.Errors.Where(e => e.Code.StartsWith("Password")));
            }

            if (result.Errors.Any(e => e.Code.StartsWith("Duplicate")))
            {
                return UserErrors.DuplicateUserName();
            }

            return UserErrors.CreateFailed(result.Errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Member);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return UserErrors.RoleAssignmentFailed(roleResult.Errors);
        }

        return user;
    }
}
