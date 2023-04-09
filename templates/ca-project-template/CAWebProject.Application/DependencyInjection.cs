using System.Reflection;
using CAWebProject.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CAWebProject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddOpenBehavior(typeof(ExceptionHandlerPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly,
            includeInternalTypes: true);

        //Turn of localization of validation errors created by Fluent Validation
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        
        return services;
    }
}