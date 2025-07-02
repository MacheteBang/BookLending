using MacheteBang.BookLending.Users.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Host.ConfigureSerilog();

var services = builder.Services;
{
    services.AddOpenApi();
    services.ConfigureHealthChecks();
    services.ConfigureTelemetry(builder.Configuration);
    services.ConfigureAuthentication(builder.Configuration);

    services.ConfigureUsers(builder.Configuration);
    services.ConfigureBooks(builder.Configuration);
}

var app = builder.Build();
{
    app.UseGlobalExceptionHandling();
    app.UseSerilogRequestLogging();

    app.UseAuth();
    app.UseHttpsRedirection();
    app.MapHealthCheckEndpoints();
    app.MapOpenApiEndpoints();

    app.UseUsers();
    app.UseBooks();
}

app.Run();

public partial class Program { }
