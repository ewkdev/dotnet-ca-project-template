using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CAWebProject.Application.Middleware;

public class LogTraceIdEnricherMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext ctx)
    {
        using (LogContext.PushProperty("TraceId", GetTraceId(ctx)))
        {
            return next.Invoke(ctx);
        }
    }

    private static string GetTraceId(HttpContext ctx)
    {
        return ctx.TraceIdentifier;
    }
}

public static class RequestCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseLogTraceIdEnricher(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogTraceIdEnricherMiddleware>();
    }
}