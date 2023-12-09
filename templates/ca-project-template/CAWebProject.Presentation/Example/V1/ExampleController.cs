using CAWebProject.Application.Features.Example.Commands;
using CAWebProject.Application.Features.Example.Queries;
using CAWebProject.Presentation.Common.Controllers;
using CAWebProject.Presentation.Example.V1.Models;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CAWebProject.Presentation.Example.V1;

[ApiController]
[Route("v{version:apiVersion}/examples")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class ExampleController : BaseController
{
    private readonly ISender _sender;
    private readonly ExampleMapper _mapper = new();
    private readonly ILogger _logger;
    
    public ExampleController(ISender mediator, ILogger<ExampleController> logger)
    {
        _sender = mediator;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExampleByIdAsync(Guid id)
    {
        var result = await _sender.Send(new GetExampleQuery { Id = id });
        return result.IsSuccess ? Ok(_mapper.ToResponse(result.Value)) : Problem(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetExamplesAsync()
    {
        var result = await _sender.Send(new GetExamplesQuery());
        return result.IsSuccess ? Ok(_mapper.ToResponse(result.Value)) : Problem(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExampleAsync(CreateExampleRequest req)
    {
        var result = await _sender.Send(_mapper.ToCommand(req));

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
        var result = await _sender.Send(new DeleteExampleCommand { Id = id });
        return result.IsSuccess ? NoContent() : Problem(result.Errors);
    }
}