using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Queries;

public class GetExamplesQueryHandler : IRequestHandler<GetExamplesQuery, Result<ExampleCollectionDto>>
{
    public async Task<Result<ExampleCollectionDto>> Handle(GetExamplesQuery request, CancellationToken cancellationToken)
    {
        //Mute compiler warning until a proper impl is provided
        await Task.CompletedTask;
        
        var collection = new ExampleCollectionDto
        {
            Total = 2,
            Offset = 0,
            Limit = 25,
            Items =
            [
                new ExampleDto
                {
                    Topic = "Im an example topic!",
                    Content = "Lorem ipsum"
                },

                new ExampleDto
                {
                    Topic = "Im the second example topic!",
                    Content = "Muspi merol"
                }
            ]
        };

        return Result.Ok(collection);
    }
}