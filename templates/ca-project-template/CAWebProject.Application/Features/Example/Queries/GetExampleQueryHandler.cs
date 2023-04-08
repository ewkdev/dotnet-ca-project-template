using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Queries;

public class GetExampleQueryHandler : IRequestHandler<GetExampleQuery ,Result<ExampleDto>>
{
    public async Task<Result<ExampleDto>> Handle(GetExampleQuery request, CancellationToken cancellationToken)
    {
        //Mute compiler warning until a proper impl is provided
        await Task.CompletedTask;
        
        var example = new ExampleDto()
        {
            Id = request.Id,
            Topic = "I am a new example with the id you provided!",
            Content = "This is just an example, normally you would query from the database."
        };

        return Result.Ok(example);
    }
}