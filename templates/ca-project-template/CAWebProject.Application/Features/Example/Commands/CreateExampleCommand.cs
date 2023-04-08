using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class CreateExampleCommand : IRequest<Result<ExampleDto>>
{
    public string Topic { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}