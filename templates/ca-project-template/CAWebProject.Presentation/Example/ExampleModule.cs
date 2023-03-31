using Asp.Versioning;
using Asp.Versioning.Builder;
using CAWebProject.Application.Features.Example.Queries;
using CAWebProject.Presentation.Example.Models.V1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CAWebProject.Presentation.Example;

using Microsoft.AspNetCore.Routing;

public static class ExampleModule
{
    public static void AddExampleEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        app.MapGet("/v{version:apiVersion}/examples", async (ISender sender) => 
            {
                var result = await sender.Send(new GetExamplesQuery());
                return Results.Ok();
            })
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(new ApiVersion(1.0));

        app.MapPost("/v{version:apiVersion}/examples", async (CreateExampleRequest req, ISender sender) =>
            {
                var mapper = new ExampleMapper();
                var cmd = await sender.Send(mapper.ToCommand(req));
                return Results.Ok();
            })
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(new ApiVersion(1.0));
    }
}