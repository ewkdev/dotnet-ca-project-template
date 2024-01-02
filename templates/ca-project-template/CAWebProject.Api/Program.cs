using CAWebProject.Api;
using CAWebProject.Application;
using CAWebProject.Application.Middleware;
using CAWebProject.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// Setup a default logger to allow for logging during ASP.Net Core startup & init
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console(theme: ConsoleTheme.None)
    .CreateBootstrapLogger();

try
{
    Log.Information("Application startup init @ {StartupTime}",
        DateTime.UtcNow.ToString("O"));

    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Host.UseSerilog((ctx, services, loggerConfig) =>
            loggerConfig
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
        );

        builder.Services.AddMvcCore();

        builder.Services
            .AddApplication()
            .AddInfrastructure()
            .AddPresentation();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    var app = builder.Build();
    {
        if (app.Configuration.GetValue<bool>("Serilog:UseRequestLogging"))
        {
            app.UseSerilogRequestLogging(cfg =>
            {
                cfg.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            });
        }

        app.UseHttpsRedirection();
        app.UseLogTraceIdEnricher();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            
            app.UseSwaggerUI(options =>
            {
                var descriptions = apiVersionDescriptionProvider.ApiVersionDescriptions;

                // Build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }

        app.MapControllers();

        Log.Information("Application startup complete @ {StartupCompleteTime}",
            DateTime.UtcNow.ToString("O"));

        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly at startup!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;