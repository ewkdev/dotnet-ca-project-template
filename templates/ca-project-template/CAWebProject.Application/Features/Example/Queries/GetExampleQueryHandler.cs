using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAWebProject.Application.Features.Example.Queries;

public class GetExampleQueryHandler(ILogger<GetExampleQueryHandler> logger)
    : IRequestHandler<GetExampleQuery, Result<ExampleDto>>
{
    public async Task<Result<ExampleDto>> Handle(
        GetExampleQuery request,
        CancellationToken cancellationToken)
    {
        //Mute compiler warning until a proper impl is provided
        await Task.CompletedTask;
        
        logger.LogInformation("Retrieving Example with id '{ExampleId}' from storage", request.Id);
        
        var example = new ExampleDto()
        {
            Id = request.Id,
            Topic = "I am a new example with the id you provided!",
            Content = "This is just an example, normally you would query from the database."
        };

        return Result.Ok(example);
    }
}