using CAWebProject.Application.Common;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CAWebProject.Presentation.Common.Controllers;

[Controller]
public abstract class BaseController : ControllerBase
{
    private static int GetErrorStatusCode(IError error)
    {
        if (!error.Metadata.TryGetValue("ErrorType", out var type))
        {
            return StatusCodes.Status500InternalServerError;
        }
        
        var errorType = (ApplicationErrorType) type;

        return errorType switch
        {
            ApplicationErrorType.Internal => StatusCodes.Status500InternalServerError,
            ApplicationErrorType.Conflict => StatusCodes.Status409Conflict,
            ApplicationErrorType.NotFound => StatusCodes.Status404NotFound,
            ApplicationErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    }
    
    protected IActionResult Problem(IEnumerable<IError> errors)
    {
        var firstError = errors.First();

        var statusCode = GetErrorStatusCode(firstError);

        return Problem(statusCode: statusCode, title: firstError.Message);
    }
}