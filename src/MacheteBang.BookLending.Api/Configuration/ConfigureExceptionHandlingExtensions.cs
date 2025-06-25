using MacheteBang.BookLending.Api.Exceptions;

namespace MacheteBang.BookLending.Api.Configuration;

public static class ConfigureExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
            exceptionHandlerApp.Run(GlobalExceptionHandler.Handle()));
    }
}
