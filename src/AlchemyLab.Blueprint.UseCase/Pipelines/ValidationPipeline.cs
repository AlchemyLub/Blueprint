namespace AlchemyLab.Blueprint.UseCase.Pipelines
{
    /// <summary>
    /// Атрибут для маркировки UseCase, который должен проходить валидацию
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ValidateRequestAttribute : Attribute
    {
    }

    /// <summary>
    /// Результат валидации
    /// </summary>
    public class ValidationResult
    {
        private readonly List<ValidationError> errors;

        /// <summary>
        /// Создает новый экземпляр результата валидации
        /// </summary>
        public ValidationResult()
        {
            errors = new List<ValidationError>();
        }

        /// <summary>
        /// Создает новый экземпляр результата валидации с указанными ошибками
        /// </summary>
        public ValidationResult(params ValidationError[] errors)
        {
            this.errors = new List<ValidationError>(errors ?? throw new ArgumentNullException(nameof(errors)));
        }

        /// <summary>
        /// Ошибки валидации
        /// </summary>
        public IReadOnlyCollection<ValidationError> Errors => errors.AsReadOnly();

        /// <summary>
        /// Показывает, является ли результат валидации успешным
        /// </summary>
        public bool IsValid => errors.Count == 0;

        /// <summary>
        /// Добавляет ошибку валидации
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public void AddError(string propertyName, string errorMessage)
        {
            errors.Add(new ValidationError(propertyName, errorMessage));
        }

        /// <summary>
        /// Добавляет ошибку валидации без привязки к свойству
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public void AddError(string errorMessage)
        {
            errors.Add(new ValidationError(string.Empty, errorMessage));
        }
    }

    /// <summary>
    /// Ошибка валидации
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Создает новый экземпляр ошибки валидации
        /// </summary>
        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Название свойства, в котором произошла ошибка
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; }
    }

    /// <summary>
    /// Исключение, возникающее при ошибке валидации
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения
        /// </summary>
        public ValidationException(ValidationResult validationResult)
            : base("Запрос не прошел валидацию.")
        {
            ValidationResult = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
        }

        /// <summary>
        /// Результат валидации, содержащий ошибки
        /// </summary>
        public ValidationResult ValidationResult { get; }
    }

    /// <summary>
    /// Интерфейс для валидатора запросов
    /// </summary>
    public interface IValidator<in TRequest>
        where TRequest : notnull
    {
        /// <summary>
        /// Валидирует запрос
        /// </summary>
        Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Пайплайн для валидации запросов перед выполнением UseCase
    /// </summary>
    [GlobalPipeline]
    public class ValidationPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Создает новый экземпляр пайплайна
        /// </summary>
        public ValidationPipeline(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Обрабатывает запрос, валидируя его перед выполнением
        /// </summary>
        public async Task<TResponse> HandleAsync(
            TRequest request,
            Func<TRequest, Task<TResponse>> next)
        {
            // Проверяем, нужно ли валидировать запрос
            // UseCase должен быть отмечен атрибутом [ValidateRequest] или запрос должен реализовывать IShouldBeValidated
            var useCaseType = next.Target?.GetType();
            var shouldValidate = (useCaseType != null && useCaseType.GetCustomAttributes(typeof(ValidateRequestAttribute), true).Length > 0) ||
                                 request is IShouldBeValidated;

            if (!shouldValidate)
            {
                // Если валидация не требуется, просто выполняем следующий шаг
                return await next(request);
            }

            // Если валидация требуется, получаем валидатор из DI
            var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TRequest));
            var validator = serviceProvider.GetService(validatorType);

            if (validator == null)
            {
                // Если валидатор не зарегистрирован, просто выполняем следующий шаг
                return await next(request);
            }

            // Вызываем метод ValidateAsync через рефлексию
            var validateMethod = validatorType.GetMethod("ValidateAsync");
            if (validateMethod == null)
            {
                // Если метод ValidateAsync не найден, пробуем найти метод Validate
                validateMethod = validatorType.GetMethod("Validate");
                if (validateMethod == null)
                {
                    throw new InvalidOperationException($"Валидатор для {typeof(TRequest).Name} не содержит метод Validate или ValidateAsync");
                }
            }

            // Вызываем метод валидации
            var validationTask = (Task)validateMethod.Invoke(validator, new object[] { request, CancellationToken.None });
            await validationTask;

            // Получаем результат валидации
            var resultProperty = validationTask.GetType().GetProperty("Result");
            if (resultProperty == null)
            {
                throw new InvalidOperationException($"Не удалось получить результат валидации для {typeof(TRequest).Name}");
            }

            var validationResult = (ValidationResult)resultProperty.GetValue(validationTask);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            // Если валидация прошла успешно, выполняем следующий шаг
            return await next(request);
        }
    }

    /// <summary>
    /// Интерфейс для запросов, которые должны проходить валидацию
    /// </summary>
    public interface IShouldBeValidated
    {
    }
}

