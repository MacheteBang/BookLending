namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static Error InvalidUserName(string providedUserName)
    {
        return Error.Validation(
            code: "Users.InvalidUserName",
            description: $"The username '{providedUserName}' provided is invalid."
        );
    }
}
