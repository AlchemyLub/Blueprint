namespace AlchemyLab.Blueprint.UseCase.Pipelines
{
    /// <summary>
    /// Пайплайн логирования запросов и результатов выполнения UseCase
    /// </summary>
    [GlobalPipeline]
    public class LoggingPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingPipeline<TRequest, TResponse>> logger;

        /// <summary>
        /// Создает новый экземпляр пайплайна логирования
        /// </summary>
        public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger) =>
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Обрабатывает запрос, логируя информацию о запросе и результате выполнения
        /// </summary>
        public async Task<TResponse> HandleAsync(
            TRequest request,
            Func<TRequest, Task<TResponse>> next)
        {
            string requestType = typeof(TRequest).Name;
            DateTime startTime = DateTime.UtcNow;

            try
            {
                TResponse result = await next(request);

                DateTime endTime = DateTime.UtcNow;

                TimeSpan duration = endTime - startTime;

                logger.LogInformation("Executed {useCase} in {duration}ms", requestType, duration.TotalMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing {useCase}: {error}", requestType, ex.Message);

                throw;
            }
        }
    }
}

