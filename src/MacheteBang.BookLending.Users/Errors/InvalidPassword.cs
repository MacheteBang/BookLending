namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static List<Error> InvalidPassword(IEnumerable<IdentityError> errors)
    {
        return errors.Select(e => Error.Validation(
            code: $"Users.InvalidPassword.{e.Code}",
            description: e.Description
        )).ToList();
    }
}
