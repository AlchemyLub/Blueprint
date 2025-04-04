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
        public async Task<TResponse> HandleAsync(
            TRequest request,
            Func<TRequest, Task<TResponse>> next)
        {
            _logger.LogInformation("Executing {useCase} with request {request}", typeof(TRequest).Name, request);

            try
            {
                var result = await next(request);
                _logger.LogInformation("Successfully executed {useCase}", typeof(TRequest).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing {useCase}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}
