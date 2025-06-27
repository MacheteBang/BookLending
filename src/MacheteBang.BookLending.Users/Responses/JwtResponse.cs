namespace MacheteBang.BookLending.Users.Responses;

public sealed record JwtResponse(
    string Token,
    DateTimeOffset ExpiresOn
);

internal static class JwtResponseExtensions
{
    public static JwtResponse ToResponse(this Jwt jwt)
    {
        return new JwtResponse(jwt.Token, jwt.ExpiresOn);
    }
}
