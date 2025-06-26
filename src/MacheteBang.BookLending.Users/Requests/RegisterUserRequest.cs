namespace MacheteBang.BookLending.Users.Requests;

public sealed record RegisterUserRequest(
    string Email,
    string Password);
