// using Asp.Versioning;
// using Asp.Versioning.Builder;
// using CAWebProject.Application.Features.Example.Commands;
// using CAWebProject.Application.Features.Example.Queries;
// using CAWebProject.Presentation.Example.V1.Models;
// using MediatR;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Http;
//
// namespace CAWebProject.Presentation.Example.V1;
//
// public static class ExampleModule
// {
//     private static readonly ExampleMapper _mapper = new();
//     private static readonly ApiVersion _v1 = new(1.0);
//
//     public static void AddExampleEndpoints(this IVersionedEndpointRouteBuilder app)
//     {
//         var v1Routes = app.MapGroup("/v{version:apiVersion}/examples").MapToApiVersion(_v1);
//
//         v1Routes.MapGet("/{id:guid}", async (ISender sender, Guid id) =>
//         {
//             var result = await sender.Send(new GetExampleQuery { Id = id });
//             return result.IsSuccess ? Results.Json(_mapper.ToResponse(result.Value)) : Results.BadRequest();
//         }).WithName("GetExample");
//
//         v1Routes.MapGet("/", async (ISender sender) =>
//         {
//             var result = await sender.Send(new GetExamplesQuery());
//             return result.IsSuccess ? Results.Json(_mapper.ToResponse(result.Value)) : Results.BadRequest();
//         });
//
//         v1Routes.MapPost("/", async (CreateExampleRequest req, ISender sender) =>
//         {
//             var result = await sender.Send(_mapper.ToCommand(req));
//             return result.IsSuccess
//                 ? Results.CreatedAtRoute(
//                     "GetExample",
//                     new { id = result.Value.Id },
//                     _mapper.ToResponse(result.Value))
//                 : Results.BadRequest();
//         });
//
//         v1Routes.MapDelete("/{id:guid}", async (ISender sender, Guid id) =>
//         {
//             var result = await sender.Send(new DeleteExampleCommand { Id = id });
//             return result.IsSuccess ? Results.NoContent() : Results.BadRequest();
//         });
//     }
// }