using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Host.ConfigureSerilog();

var services = builder.Services;
{
    services.AddOpenApi();
    services.ConfigureHealthChecks();
    services.ConfigureTelemetry(builder.Configuration);
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapHealthCheckEndpoints();
    app.MapOpenApiEndpoints();
}

app.Run();
