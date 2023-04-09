using Asp.Versioning;
using CAWebProject.Application;
using CAWebProject.Infrastructure;
using CAWebProject.Presentation;
using CAWebProject.Presentation.Example;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddApplication()
        .AddInfrastructure()
        .AddPresentation();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    var apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1.0))
        .ReportApiVersions()
        .Build();
    
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.NewVersionedApi("Example Api")
        .AddExampleEndpoints();

    app.Run();
}