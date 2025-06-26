namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static Error UserNotFound(Guid userId) => Error.NotFound(
        code: "Users.UserNotFound",
        description: $"The specified user with id of {userId} was not found.");
}
