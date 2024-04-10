using CAWebProject.Api;
using CAWebProject.Api.Swagger;
using CAWebProject.Application;
using CAWebProject.Application.Middleware;
using CAWebProject.Infrastructure;

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
        //don't expose tech stack details to clients
        builder.WebHost.UseKestrel(opts =>
        {
            opts.AddServerHeader = false;
        });
        
        builder.Host.UseSerilog((ctx, services, loggerConfig) =>
            loggerConfig
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
        );

        // allow nameof(Action) to be found even if it has an Async as its suffix
        builder.Services.AddMvcCore(opts =>
        {
            opts.SuppressAsyncSuffixInActionNames = false;
        });

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
            app.UseSwaggerDocumentation();
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