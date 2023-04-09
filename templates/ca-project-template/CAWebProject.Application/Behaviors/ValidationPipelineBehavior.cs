using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CAWebProject.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest,TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var ctx = new ValidationContext<TRequest>(request);

        var errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (!errors.Any())
        {
            return await next();
        }
        
        _logger.LogError("Validation failed! {ErrorCount} validation failures have occured", errors.Count);

        var result = Result.Fail("One or more validation failures occured.");
        
        errors.ForEach(error => result.WithError($"{error.PropertyName}:{error.ErrorMessage}"));

        return (dynamic) result;
    }
}