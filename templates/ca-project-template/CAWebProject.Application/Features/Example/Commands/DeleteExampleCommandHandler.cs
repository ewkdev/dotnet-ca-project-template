using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class DeleteExampleCommandHandler : IRequestHandler<DeleteExampleCommand, Result>
{
    public async Task<Result> Handle(DeleteExampleCommand request, CancellationToken cancellationToken)
    {
        //Mute compiler warning until a proper impl is provided
        await Task.CompletedTask;
        
        return Result.Ok();
    }
}