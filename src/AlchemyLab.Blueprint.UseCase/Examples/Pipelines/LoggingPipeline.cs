using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AlchemyLab.Blueprint.UseCase.Examples.Pipelines
{
    /// <summary>
    /// Пайплайн для логирования выполнения UseCase
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    [GlobalPipeline]
    public class LoggingPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> HandleAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
        {
            _logger.LogInformation("Executing {UseCase} with request {Request}", typeof(TRequest).Name, request);

            try
            {
                TResponse result = await next(request);

                _logger.LogInformation("Successfully executed {UseCase}", typeof(TRequest).Name);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing {UseCase}", typeof(TRequest).Name);

                throw;
            }
        }
    }
}
