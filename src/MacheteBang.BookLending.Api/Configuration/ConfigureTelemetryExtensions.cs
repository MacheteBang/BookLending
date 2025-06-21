using Azure.Monitor.OpenTelemetry.AspNetCore;
using MacheteBang.BookLending.Api.Telemetry;
using OpenTelemetry.Trace;

namespace MacheteBang.BookLending.Api.Configuration;

public static class ConfigureTelemetryExtensions
{
    public static IServiceCollection ConfigureTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        string? appInsightsConnectionString = configuration.GetConnectionString("ApplicationInsights");
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            services
                .AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder.AddAspNetCoreInstrumentation();
                    builder.AddEntityFrameworkCoreInstrumentation();
                    builder.AddProcessor(sp =>
                        new HealthCheckTelemetryProcessor(sp.GetRequiredService<ILogger<HealthCheckTelemetryProcessor>>()));
                })
                .UseAzureMonitor(options =>
                {
                    options.ConnectionString = appInsightsConnectionString;
                });
        }

        return services;
    }
}
