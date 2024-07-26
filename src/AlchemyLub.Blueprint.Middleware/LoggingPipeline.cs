namespace AlchemyLub.Blueprint.Middleware;

public sealed class LoggingPipeline<T>(ILogger<LoggingPipeline<T>> logger) : IPipeline<T> where T : class
{
    /// <inheritdoc />
    public async Task Invoke(T service, Func<T, Func<Task>> next)
    {
        logger.LogInformation("Pipeline {PipelineType} is starting", typeof(T).Name);

        await next(service)();

        logger.LogInformation("Pipeline {PipelineType} is ending", typeof(T).Name);
    }
}
