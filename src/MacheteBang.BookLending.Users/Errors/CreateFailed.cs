namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static List<Error> CreateFailed(IEnumerable<IdentityError> errors)
    {
        return errors.Select(e => Error.Validation(
            code: $"Users.CreateFailed.{e.Code}",
            description: e.Description
        )).ToList();
    }
}
