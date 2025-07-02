namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static Error DuplicateUserName()
    {
        return Error.Conflict(
            code: "Users.DuplicateUserName",
            description: "A user with this username already exists."
        );
    }
}
