using System.Reflection;

namespace MacheteBang.BookLending.Books.Configuration;

public static class ConfigureBooksExtensions
{
    public static IServiceCollection ConfigureBooks(this IServiceCollection services) => services;
    public static WebApplication MapBooksEndpoints(this WebApplication app)
    {
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        var endpoints = thisAssembly.GetTypes()
            .Where(t => typeof(IBooksEndpoint).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IBooksEndpoint;
            instance?.MapBooksEndpoint(app);
        }

        return app;
    }
}
