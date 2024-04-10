using Asp.Versioning;
using CAWebProject.Api.Common.Controllers;
using CAWebProject.Api.Example.V1.Models;
using CAWebProject.Application.Features.Example.Commands;
using CAWebProject.Application.Features.Example.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CAWebProject.Api.Example.V2;

[ApiController]
[ControllerName("Example")]
[Route("v{version:apiVersion}/examples")]
[Produces("application/json")]
[ApiVersion("2.0")]
public class ExampleControllerV2(ISender mediator, ILogger<ExampleControllerV2> logger) : BaseController
{
    private readonly ExampleMapper _mapper = new();

    [HttpGet("{id:guid}", Name = nameof(GetExampleByIdAsync))]
    public async Task<IActionResult> GetExampleByIdAsync(Guid id)
    {
        var result = await mediator.Send(new GetExampleQuery { Id = id });
        return result.IsSuccess ? Ok(_mapper.ToResponse(result.Value)) : Problem(result.Errors);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetExamplesAsync()
    {
        var result = await mediator.Send(new GetExamplesQuery());
        return result.IsSuccess ? Ok(_mapper.ToResponse(result.Value)) : Problem(result.Errors);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExampleAsync(CreateExampleRequest req)
    {
        var result = await mediator.Send(_mapper.ToCommand(req));

        return result.IsSuccess
            ? CreatedAtRoute(
                nameof(GetExampleByIdAsync),
                new { id = result.Value.Id },
                _mapper.ToResponse(result.Value))
            : Problem(result.Errors);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExampleAsync(Guid id)
    {
        var result = await mediator.Send(new DeleteExampleCommand { Id = id });
        return result.IsSuccess ? NoContent() : Problem(result.Errors);
    }
}