using Serilog;

namespace CAWebProject.Api.Common.BuilderExtentions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureHost(this WebApplicationBuilder builder)
    {
        //don't expose tech stack details to clients
        builder.WebHost.UseKestrel(opts =>
        {
            opts.AddServerHeader = false;
        });
    }

    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, services, loggerConfig) =>
            loggerConfig
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
        );
    }
}