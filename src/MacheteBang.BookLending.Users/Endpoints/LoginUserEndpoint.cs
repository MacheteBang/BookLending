using MacheteBang.BookLending.Users.Services;

namespace MacheteBang.BookLending.Users.Endpoints;

internal sealed class LoginUserEndpoint : IUsersEndpoint
{
    public void MapUsersEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPost("/login", async ([FromServices] SignInManager<User> signInManager,
                                      [FromServices] UserManager<User> userManager,
                                      [FromServices] IJwtService jwtService,
                                      LoginUserRequest request) =>
            {
                var result = await LoginUserAsync(signInManager, userManager, jwtService, request);
                return result.Match(
                    token => Results.Ok(token.ToResponse()),
                    errors => errors.ToProblemResult());
            })
            .Produces<JwtResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Logs in a user and returns a JWT")
            .WithName("LoginUser")
            .WithSummary("Login User");
    }

    private static async Task<ErrorOr<Jwt>> LoginUserAsync(SignInManager<User> signInManager, UserManager<User> userManager, IJwtService jwtService, LoginUserRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null) return UserErrors.InvalidLoginAttempt();

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut) return new List<Error> { Error.Validation("Users.LockedOut", "User account is locked out.") };
            if (signInResult.IsNotAllowed) return new List<Error> { Error.Validation("Users.NotAllowed", "User is not allowed to sign in.") };
            return UserErrors.InvalidLoginAttempt();
        }

        var roles = await userManager.GetRolesAsync(user);
        var tokenString = jwtService.CreateToken(user.Id, user.UserName, user.Email, roles);
        var expires = jwtService.GetExpiration();
        return new Jwt(tokenString, expires);
    }
}
