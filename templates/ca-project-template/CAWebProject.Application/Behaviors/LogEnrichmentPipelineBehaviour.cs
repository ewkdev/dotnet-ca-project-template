using System.Diagnostics;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace CAWebProject.Application.Behaviors;

public class LogEnrichmentPipelineBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    private readonly ILogger<LogEnrichmentPipelineBehaviour<TRequest, TResponse>> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public LogEnrichmentPipelineBehaviour(
        ILogger<LogEnrichmentPipelineBehaviour<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor,
        IDiagnosticContext serilogDiagnosticContext)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _diagnosticContext = serilogDiagnosticContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? _httpContextAccessor.HttpContext?.TraceIdentifier;
        
        //Set TraceId log property for this request
        LogContext.Push(new TraceIdEnricher(traceId));
        
        //Set TraceId log property for Serilog's HTTP Request Logging feature
        _diagnosticContext.Set("TraceId", traceId);
        
        var result = await next();

        return result;
    }
}

internal class TraceIdEnricher : ILogEventEnricher
{
    private readonly string? _traceId;

    internal TraceIdEnricher(string? traceId)
    {
        _traceId = traceId;
    }
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        
        if (string.IsNullOrWhiteSpace(_traceId))
        {
            return;
        }
        
        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty("TraceId", _traceId));
    }
}