namespace MacheteBang.BookLending.Users.Errors;

internal static partial class UserErrors
{
    public static List<Error> RoleAssignmentFailed(IEnumerable<IdentityError> errors)
    {
        return errors.Select(e => Error.Validation(
            code: $"Users.RoleAssignmentFailed.{e.Code}",
            description: e.Description
        )).ToList();
    }
}
