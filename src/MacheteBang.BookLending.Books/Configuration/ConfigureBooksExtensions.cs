using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MacheteBang.BookLending.Books.Configuration;

public static class ConfigureBooksExtensions
{
    public static IServiceCollection ConfigureBooks(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("BooksDb");
        if (!string.IsNullOrEmpty(dbConnectionString))
        {
            services.AddDbContext<BooksDbContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            });
        }

        return services;
    }

    public static WebApplication UseBooks(this WebApplication app)
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
