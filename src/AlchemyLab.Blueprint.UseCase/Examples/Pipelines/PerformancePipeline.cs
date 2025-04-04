using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AlchemyLab.Blueprint.UseCase.Examples.Pipelines
{
    /// <summary>
    /// Пайплайн для измерения производительности выполнения UseCase
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    [GlobalPipeline]
    public class PerformancePipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<PerformancePipeline<TRequest, TResponse>> _logger;
        private readonly int _thresholdMs;

        public PerformancePipeline(ILogger<PerformancePipeline<TRequest, TResponse>> logger, int thresholdMs)
        {
            _logger = logger;
            _thresholdMs = thresholdMs;
        }

        /// <inheritdoc />
        public async Task<TResponse> HandleAsync(
            TRequest request,
            Func<TRequest, Task<TResponse>> next)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next(request);
                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;
                if (elapsed > _thresholdMs)
                {
                    _logger.LogWarning("Выполнение UseCase {RequestType} заняло {ElapsedMs}мс, что превышает порог {ThresholdMs}мс",
                        typeof(TRequest).Name, elapsed, _thresholdMs);
                }
                else
                {
                    _logger.LogInformation("Выполнение UseCase {RequestType} заняло {ElapsedMs}мс",
                        typeof(TRequest).Name, elapsed);
                }

                return result;
            }
            catch (Exception)
            {
                stopwatch.Stop();
                _logger.LogWarning("Выполнение UseCase {RequestType} завершилось ошибкой через {ElapsedMs}мс",
                    typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
