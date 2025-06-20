using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();

var services = builder.Services;
{
    services.AddOpenApi();
    services.ConfigureHealthChecks();
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.MapHealthCheckEndpoints();
    app.MapOpenApiEndpoints();
}


app.Run();
