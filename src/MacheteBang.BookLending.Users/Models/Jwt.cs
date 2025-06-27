namespace MacheteBang.BookLending.Users.Models;

internal sealed record Jwt(string Token, DateTimeOffset ExpiresOn);
