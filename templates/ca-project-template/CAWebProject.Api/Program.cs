using CAWebProject.Application;
using CAWebProject.Infrastructure;
using CAWebProject.Presentation;
using CAWebProject.Presentation.Example;
using CAWebProject.Presentation.Example.V1;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Serilog;
using Serilog.Events;

// Setup a default logger to allow for logging during ASP.Net Core startup & init
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
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
        app.UseSerilogRequestLogging(cfg =>
        {
            cfg.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        });
        app.UseHttpsRedirection();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
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