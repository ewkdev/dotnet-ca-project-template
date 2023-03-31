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

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddValidatorsFromAssembly(applicationAssembly,
            includeInternalTypes: true);

        //Turn of localization of validation errors created by Fluent Validation
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        
        return services;
    }
}