using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Queries;

public class GetExamplesQueryHandler : IRequestHandler<GetExamplesQuery, Result>
{
    public async Task<Result> Handle(GetExamplesQuery request, CancellationToken cancellationToken)
    {
        return Result.Ok();
    }
}