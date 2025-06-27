using System.Reflection;
using Microsoft.Extensions.Configuration;
using MacheteBang.BookLending.Users.Services;

namespace MacheteBang.BookLending.Users.Configuration;

public static class ConfigureUsersExtensions
{
    public static IServiceCollection ConfigureUsers(this IServiceCollection services, IConfiguration configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("UsersDb");
        if (!string.IsNullOrEmpty(dbConnectionString))
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            });
        }

        // Register the Identity services
        services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders();

        // Register JwtService
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }

    public static WebApplication UseUsers(this WebApplication app)
    {
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        var endpoints = thisAssembly.GetTypes()
            .Where(t => typeof(IUsersEndpoint).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IUsersEndpoint;
            instance?.MapUsersEndpoint(app);
        }

        return app;
    }
}
