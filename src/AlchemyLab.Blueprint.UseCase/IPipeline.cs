namespace AlchemyLab.Blueprint.UseCase;

/// <summary>
/// Интерфейс для пайплайна, который обрабатывает запрос перед выполнением UseCase
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
public interface IPipeline<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Обрабатывает запрос и передает его дальше по цепочке
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="next">Следующий обработчик в цепочке</param>
    /// <returns>Результат обработки</returns>
    Task<TResponse> HandleAsync(
        TRequest request,
        Func<TRequest, Task<TResponse>> next);
}
