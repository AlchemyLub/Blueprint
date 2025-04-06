namespace AlchemyLab.Blueprint.UseCase.Examples.Pipelines;

/// <summary>
/// Пайплайн для валидации запросов
/// </summary>
/// <typeparam name="TRequest">Тип запроса</typeparam>
/// <typeparam name="TResponse">Тип ответа</typeparam>
[GlobalPipeline]
public class ValidationPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> where TRequest : notnull
{
    /// <inheritdoc />
    public async Task<TResponse> HandleAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Проверяем, реализует ли запрос интерфейс IValidatable
        if (request is IValidatable validatable)
        {
            // Валидируем запрос
            ValidationResult validationResult = validatable.Validate();

            if (!validationResult.IsValid)
            {
                // Если валидация не прошла, выбрасываем исключение
                throw new ValidationException($"Запрос не прошел валидацию: {string.Join(", ", validationResult.Errors)}");
            }
        }

        // Если запрос прошел валидацию или не требует валидации, выполняем следующий шаг
        return await next(request);
    }
}

/// <summary>
/// Интерфейс для запросов, которые могут быть валидированы
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// Валидирует запрос
    /// </summary>
    ValidationResult Validate();
}

/// <summary>
/// Исключение валидации
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}
