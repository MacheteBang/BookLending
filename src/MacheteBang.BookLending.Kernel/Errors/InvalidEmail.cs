using ErrorOr;

namespace MacheteBang.BookLending.Kernel.Errors;

public static partial class KernelErrors
{
    public static Error InvalidEmail(string providedEmail)
    {
        return Error.Validation(
            code: "Kernel.InvalidEmail",
            description: $"The email '{providedEmail}' provided is invalid."
        );
    }
}
