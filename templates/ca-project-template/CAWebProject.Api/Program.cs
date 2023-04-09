using CAWebProject.Application;
using CAWebProject.Infrastructure;
using CAWebProject.Presentation;
using CAWebProject.Presentation.Example;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddMvcCore();
    
    builder.Services
        .AddApplication()
        .AddInfrastructure()
        .AddPresentation();
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.NewVersionedApi("Example Api")
        .AddExampleEndpoints();
        //Chain additional Modules here
        
    app.Run();
}