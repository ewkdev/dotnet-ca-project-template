using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class DeleteExampleCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}