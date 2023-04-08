using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class DeleteExampleCommand : IRequest<Result>
{
    public int Id { get; set; }
}