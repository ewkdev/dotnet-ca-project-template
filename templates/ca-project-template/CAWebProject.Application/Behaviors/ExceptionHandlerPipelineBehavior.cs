using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAWebProject.Application.Behaviors;

public class ExceptionHandlerPipelineBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlerPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogCritical("""
                               An unhandled exception was thrown during the execution of this request.
                               The exception message was: {ExceptionMessage}
                               
                               {ExceptionStackTrace}
                               """, 
                ex.Message, ex.StackTrace);

            return (dynamic) Result.Fail("Unhandled Exception occured while handling this request.");
        }
    }
}