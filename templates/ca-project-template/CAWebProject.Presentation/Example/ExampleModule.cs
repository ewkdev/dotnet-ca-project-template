using Asp.Versioning;
using Asp.Versioning.Builder;
using CAWebProject.Application.Features.Example.Commands;
using CAWebProject.Application.Features.Example.Queries;
using CAWebProject.Presentation.Example.Models.V1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CAWebProject.Presentation.Example;

using Microsoft.AspNetCore.Routing;

public static class ExampleModule
{
    private static readonly ExampleMapper _mapper = new ();
    private static readonly ApiVersion _v1 = new (1.0);
    
    public static void AddExampleEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        app.MapGet("/v{version:apiVersion}/examples/{id:int}", async (ISender sender, int id) => 
            {
                var result = await sender.Send(new GetExampleQuery { Id = id });
                return result.IsSuccess ?
                    Results.Json(_mapper.ToResponse(result.Value)) :
                    Results.BadRequest();
            })
            .WithName("GetExample")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(_v1);
        
        app.MapGet("/v{version:apiVersion}/examples", async (ISender sender) => 
            {
                var result = await sender.Send(new GetExamplesQuery());
                return result.IsSuccess ?
                    Results.Json(_mapper.ToResponse(result.Value)) :
                    Results.BadRequest();
            })
            .WithName("GetExamples")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(_v1);

        app.MapPost("/v{version:apiVersion}/examples", async (CreateExampleRequest req, ISender sender) =>
            {
                var result = await sender.Send(_mapper.ToCommand(req));
                return result.IsSuccess ?
                    Results.CreatedAtRoute(
                        "GetExample", 
                        new { id = result.Value.Id },
                        _mapper.ToResponse(result.Value)) :
                    Results.BadRequest();
            })
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(_v1);

        app.MapDelete("/v{version:apiVersion}/examples/{id:int}", async (ISender sender, int id) =>
            {
                var result = await sender.Send(new DeleteExampleCommand { Id = id });
                return result.IsSuccess ? Results.NoContent() : Results.BadRequest();
            })
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(_v1);
    }
}