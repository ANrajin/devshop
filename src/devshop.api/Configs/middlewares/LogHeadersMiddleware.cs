namespace devshop.api.Configs.Middlewares;

public class LogHeadersMiddleware(RequestDelegate next,
    ILogger<LogHeadersMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private ILogger<LogHeadersMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation("Header: {Key} - {Value}", header.Key, header.Value);
        }

        await _next(context);
    }
}

public static class LogHeaderMiddlewareExtension
{
    public static IApplicationBuilder UseLogHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogHeadersMiddleware>();
    }
}
