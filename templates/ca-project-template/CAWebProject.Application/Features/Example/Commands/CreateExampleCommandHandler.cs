using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, Result<ExampleDto>>
{
    public async Task<Result<ExampleDto>> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        //Mute compiler warning until a proper impl is provided
        await Task.CompletedTask;
        
        var mapper = new ExampleMapper();
        var example = new Project.Domain.Example.Example { Id = 456, Topic = $"I am a Domain {request.Topic} Topic", Content = request.Content};
        return Result.Ok(mapper.ToDto(example));
    }
}