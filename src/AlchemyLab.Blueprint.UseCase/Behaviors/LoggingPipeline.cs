namespace AlchemyLab.Blueprint.UseCase.Behaviors;

/// <summary>
/// Поведение для логирования выполнения UseCase'ов
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
[GlobalPipeline]
public class LoggingPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingPipeline<TRequest, TResponse>> logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="LoggingPipeline{TRequest, TResponse}"/>
    /// </summary>
    public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger) => this.logger = logger;

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        logger.LogInformation("Executing {UseCase} with request {Request}", typeof(TRequest).Name, request);

        try
        {
            var result = await next(request);

            logger.LogInformation("Successfully executed {UseCase}", typeof(TRequest).Name);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing {UseCase}", typeof(TRequest).Name);

            throw;
        }
    }
}
