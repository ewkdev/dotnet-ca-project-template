using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Commands;

public class CreateExampleCommand : IRequest<Result>
{
    public string Topic { get; set; }
    
    public string Content { get; set; }
}