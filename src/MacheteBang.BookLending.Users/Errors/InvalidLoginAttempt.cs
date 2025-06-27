namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static Error InvalidLoginAttempt() => Error.Failure(
        code: "Users.InvalidLoginAttempt",
        description: "The login attempt was invalid.");
}
