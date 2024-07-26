namespace AlchemyLub.Blueprint.App.Middlewares;

/// <summary>
/// Middleware that logs the request context.
/// </summary>
public sealed class RequestContextLoggingMiddleware(ILogger<RequestContextLoggingMiddleware> logger) : IMiddleware
{
    /// <summary>
    /// The name of the header that contains the correlation ID.
    /// </summary>
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    /// <inheritdoc />
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string correlationId = GetCorrelationId(context);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Нужны нормальные логи, чтобы понять, что происходит
            logger.LogInformation("Processing request {RequestName}", context.Request.Method);

            // Переписать
            Task result = next(context);

            // Нужны нормальные логи, чтобы понять, что происходит
            logger.LogInformation("Completed request {RequestName}", context.Request.Method);

            return result;
        }
    }

    /// <summary>
    /// Gets the correlation ID from the HTTP context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>The correlation ID.</returns>
    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? Guid.NewGuid().ToString();
    }
}
