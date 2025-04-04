namespace AlchemyLab.Blueprint.UseCase;

/// <summary>
/// Базовый класс для UseCase с поддержкой пайплайнов
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
public abstract class UseCase<TRequest, TResponse> : IUseCase<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly UseCaseContext context;

    /// <summary>
    /// Конструктор с инъекцией контекста
    /// </summary>
    /// <param name="context">Контекст юзкейса</param>
    protected UseCase(UseCaseContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Выполняет UseCase, применяя все зарегистрированные пайплайны
    /// </summary>
    public virtual Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        return context.ApplyPipelines(
            request,
            ExecuteCore,
            GetType(),
            cancellationToken);
    }

    /// <summary>
    /// Основная реализация UseCase, которую нужно переопределить в наследниках
    /// </summary>
    protected abstract Task<TResponse> ExecuteCore(TRequest request);
}

/// <summary>
/// Базовый класс для UseCase без возвращаемого значения
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
public abstract class UseCase<TRequest> : IUseCase<TRequest>
    where TRequest : notnull
{
    private readonly UseCaseContext context;

    /// <summary>
    /// Конструктор с инъекцией контекста
    /// </summary>
    /// <param name="context">Контекст юзкейса</param>
    protected UseCase(UseCaseContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Выполняет UseCase применяя все зарегистрированные пайплайны
    /// </summary>
    public virtual Task Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        // Применяем все пайплайны к запросу и выполняем основную логику
        return context.ApplyPipelines(
            request,
            ExecuteCore,
            GetType(),
            cancellationToken);
    }

    /// <summary>
    /// Основная логика UseCase
    /// </summary>
    protected abstract Task<ValueTuple> ExecuteCore(TRequest request);
}
