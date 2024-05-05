using Asp.Versioning;

namespace CAWebProject.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // allow nameof(Action) to be found even if it has an Async as its suffix
        services.AddControllers(opts =>
        {
            opts.SuppressAsyncSuffixInActionNames = false;
        });
        
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'VVV";
            opts.SubstituteApiVersionInUrl = true;
        });
        
        services.AddProblemDetails();
        
        return services;
    }
}