namespace AlchemyLab.Blueprint.UseCase.Behaviors;

/// <summary>
/// Интерфейс для валидаторов запросов
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
public interface IValidator<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Валидирует запрос
    /// </summary>
    /// <param name="request">Запрос для валидации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат валидации</returns>
    Task<ValidationResult> Validate(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Результат валидации
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Список ошибок валидации
    /// </summary>
    public List<ValidationError> Errors { get; } = new();

    /// <summary>
    /// Признак успешной валидации
    /// </summary>
    public bool IsValid => Errors.Count == 0;

    /// <summary>
    /// Добавляет ошибку валидации
    /// </summary>
    /// <param name="propertyName">Имя свойства</param>
    /// <param name="errorMessage">Сообщение об ошибке</param>
    public void AddError(string propertyName, string errorMessage)
    {
        Errors.Add(new ValidationError(propertyName, errorMessage));
    }
}

/// <summary>
/// Ошибка валидации
/// </summary>
/// <param name="PropertyName">Имя свойства</param>
/// <param name="ErrorMessage">Сообщение об ошибке</param>
public record ValidationError(string PropertyName, string ErrorMessage);

/// <summary>
/// Исключение при ошибке валидации
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Создает новый экземпляр исключения
    /// </summary>
    /// <param name="validationResult">Результат валидации</param>
    public ValidationException(ValidationResult validationResult)
        : base("Validation failed")
    {
        ValidationResult = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
    }

    /// <summary>
    /// Результат валидации
    /// </summary>
    public ValidationResult ValidationResult { get; }
}

/// <summary>
/// Поведение для валидации запросов
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
[GlobalPipeline]
public class ValidationBehavior<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ValidationBehavior{TRequest, TResponse}"/>
    /// </summary>
    public ValidationBehavior(IServiceProvider serviceProvider, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(
        TRequest request,
        Func<TRequest, Task<TResponse>> next)
    {
        logger.LogDebug("Validating request {RequestType}", typeof(TRequest).Name);

        // Проверка через валидатор
        Pipelines.IValidator<TRequest>? validator = serviceProvider.GetService<Pipelines.IValidator<TRequest>>();

        if (validator is not null)
        {
            Pipelines.ValidationResult result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                logger.LogWarning("Validation failed for {RequestType}", typeof(TRequest).Name);
                throw new Pipelines.ValidationException(result);
            }
        }

        // Проверка через IValidatable
        if (request is IValidatable validatable)
        {
            try
            {
                validatable.Validate();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Validation failed for {RequestType}", typeof(TRequest).Name);
                throw;
            }
        }

        logger.LogDebug("Validation successful for {RequestType}", typeof(TRequest).Name);
        return await next(request);
    }
}

/// <summary>
/// Интерфейс для объектов, которые могут валидировать себя
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// Валидирует объект и выбрасывает исключение при ошибках
    /// </summary>
    void Validate();
}
