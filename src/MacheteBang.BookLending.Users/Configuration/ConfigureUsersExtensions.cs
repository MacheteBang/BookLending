using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MacheteBang.BookLending.Users.Services;
using System.Text;

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

        // JWT Authentication setup
        var jwtSection = configuration.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseUsers(this WebApplication app)
    {
        app.UseAuthentication(); // Ensure authentication middleware is added
        app.UseAuthorization();  // Ensure authorization middleware is added

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
