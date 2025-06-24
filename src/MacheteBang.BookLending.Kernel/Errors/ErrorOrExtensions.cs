using Microsoft.AspNetCore.Http;
using ErrorOr;

namespace MacheteBang.BookLending.Kernel.Errors;

public static class ErrorOrExtensions
{
    public static IResult ToProblemResult(this ICollection<Error> errors)
    {
        if (!errors.All(e => e.Type == ErrorType.Validation))
            return errors.First().ToProblemResult();

        // They are all validation errors, so we can return a 400 with details
        var errorDict = errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray()
            );
        return Results.ValidationProblem(errorDict);
    }

    public static IResult ToProblemResult(this Error error)
    {
        if (error.Type == ErrorType.Validation) return ToProblemResult([error]);

        // Map error to a ProblemDetails response
        return Results.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            }
        );
    }
}
