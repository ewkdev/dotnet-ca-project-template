using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAWebProject.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var ctx = new ValidationContext<TRequest>(request);

        var errors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (errors.Count == 0)
        {
            return await next();
        }
        
        logger.LogError("Validation failed! {ErrorCount} validation failures have occured", errors.Count);

        var result = Result.Fail("One or more validation failures occured.");
        
        errors.ForEach(error => result.WithError($"{error.PropertyName}:{error.ErrorMessage}"));

        return (dynamic) result;
    }
}