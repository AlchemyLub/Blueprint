namespace AlchemyLab.Blueprint.UseCase.Examples.Pipelines;

/// <summary>
/// Пайплайн для измерения производительности выполнения UseCase
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
[GlobalPipeline]
public class PerformancePipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<PerformancePipeline<TRequest, TResponse>> logger;
    private readonly int thresholdMs;

    public PerformancePipeline(ILogger<PerformancePipeline<TRequest, TResponse>> logger, int thresholdMs)
    {
        this.logger = logger;
        this.thresholdMs = thresholdMs;
    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            TResponse result = await next(request);

            stopwatch.Stop();

            long elapsed = stopwatch.ElapsedMilliseconds;

            if (elapsed > thresholdMs)
            {
                logger.LogWarning("Выполнение UseCase {RequestType} заняло {ElapsedMs}мс, что превышает порог {ThresholdMs}мс",
                    typeof(TRequest).Name, elapsed, thresholdMs);
            }
            else
            {
                logger.LogInformation("Выполнение UseCase {RequestType} заняло {ElapsedMs}мс", typeof(TRequest).Name, elapsed);
            }

            return result;
        }
        catch (Exception)
        {
            stopwatch.Stop();

            logger.LogWarning("Выполнение UseCase {RequestType} завершилось ошибкой через {ElapsedMs}мс",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
