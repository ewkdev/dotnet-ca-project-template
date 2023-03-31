using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class CreateExampleCommandHandler : IRequestHandler<CreateExampleCommand, Result>
{
    public async Task<Result> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
    {
        return Result.Ok();
    }
}