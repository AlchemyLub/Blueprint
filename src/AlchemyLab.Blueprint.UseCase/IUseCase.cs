namespace AlchemyLab.Blueprint.UseCase;

/// <summary>
/// Базовый интерфейс для UseCase с возвращаемым значением
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
public interface IUseCase<in TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет UseCase
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат выполнения</returns>
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Интерфейс для UseCase без возвращаемого значения
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
public interface IUseCase<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Выполняет UseCase
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task Execute(TRequest request, CancellationToken cancellationToken = default);
}
