namespace MacheteBang.BookLending.Users.Requests;

public sealed record LoginUserRequest(
    string UserName,
    string Password);
