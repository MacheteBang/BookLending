using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MacheteBang.BookLending.Api.Exceptions;

public static class GlobalExceptionHandler
{
    public static RequestDelegate Handle()
    {
        return async context =>
        {
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature?.Error;

            if (exception is null) return;

            // Get logger from DI and log the exception
            var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger(nameof(GlobalExceptionHandler));
            logger?.LogError(exception,
                "Unhandled exception occurred. Type: {ExceptionType}, Message: {Message}, Path: {Path}, StackTrace: {StackTrace}",
                exception.GetType().FullName,
                exception.Message,
                context.Request.Path,
                exception.StackTrace);

            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error",
                Detail = "Something went wrong on our end. Please try again later or contact support if the issue persists.",
                Instance = context.Request.Path
            };

            if (exception is BadHttpRequestException)
            {
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "An invalid request was made";
                problemDetails.Detail = exception.Message;
                problemDetails.Instance = context.Request.Path;
            }

            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = "application/problem+json";

            await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
        };
    }
}
