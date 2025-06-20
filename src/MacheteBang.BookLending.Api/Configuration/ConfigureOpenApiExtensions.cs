using Scalar.AspNetCore;

namespace MacheteBang.BookLending.Api.Configuration;

internal static class ConfigureOpenApiExtensions
{
    internal static WebApplication MapOpenApi(this WebApplication app)
    {
        OpenApiEndpointRouteBuilderExtensions.MapOpenApi(app);
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("MacheteBang Book Lending API")
                .WithTheme(ScalarTheme.Laserwave)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}
