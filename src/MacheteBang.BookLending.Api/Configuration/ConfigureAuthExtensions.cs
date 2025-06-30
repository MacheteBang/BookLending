using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MacheteBang.BookLending.Api.Configuration;

internal static class ConfigureAuthExtensions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Introduce the GetRequired Extensinsion method to IConfiguration
        var jwtSection = configuration.GetRequiredSection("Jwt");
        string key = jwtSection["Key"] ?? throw new ArgumentNullException("Jwt Key is missing in configuration");
        string issuer = jwtSection["Issuer"] ?? throw new ArgumentNullException("Jwt Issuer is missing in configuration");
        string audience = jwtSection["Audience"] ?? throw new ArgumentNullException("Jwt Audience is missing in configuration");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
