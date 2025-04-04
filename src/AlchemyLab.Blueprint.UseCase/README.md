# AlchemyLab.Blueprint.UseCase

Легковесная альтернатива MediatR для организации UseCase'ов в .NET приложениях без лишних абстракций.

## Особенности

- Простой дизайн без лишних абстракций
- **Абсолютно чистые UseCase'ы** без конструкторов и зависимостей от инфраструктуры
- Простое использование UseCase'ов напрямую через DI
- Поддержка UseCase'ов с возвращаемым значением и без него
- Поддержка пайплайнов с гибкой фильтрацией:
  - Глобальные пайплайны (для всех UseCase'ов)
  - Специфичные пайплайны (для конкретных UseCase'ов)
- Встроенные пайплайны для логирования и валидации
- Легко расширяемая архитектура

## Установка

```bash
dotnet add package AlchemyLab.Blueprint.UseCase
```

## Использование

### Регистрация сервисов

```csharp
// Настройка сервисов
ServiceCollection services = new();

// Регистрируем UseCase'ы
services.AddUseCase<GetUser>();
services.AddUseCase<UpdateUser>();

// Или регистрируем все UseCase'ы из сборки
services.AddUseCasesFromAssembly(typeof(GetUser).Assembly);

// Регистрируем пайплайны
// Глобальный - применяется ко всем UseCase'ам
services.AddGlobalPipeline<LoggingBehavior<GetUserRequest, GetUserResponse>>();

// Специфичный - только для определенного UseCase'а
services.AddPipelineFor<SomeBehavior<GetUserRequest, GetUserResponse>, GetUser>();

// Создаем ServiceProvider с автоматической инициализацией UseCase контекста
ServiceProvider serviceProvider = services.BuildUseCaseProvider();
```

### Создание UseCase'а с возвращаемым значением

```csharp
// Запрос
public class GetUserRequest
{
    public Guid UserId { get; set; }
}

// Ответ
public class GetUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// UseCase - абсолютно чистый, содержит только бизнес-логику
// Нет конструкторов, нет зависимостей от инфраструктуры
public class GetUser : UseCase<GetUserRequest, GetUserResponse>
{
    protected override async Task<GetUserResponse> ExecuteCore(GetUserRequest request, CancellationToken cancellationToken)
    {
        // Только бизнес-логика
        return new GetUserResponse
        {
            Id = request.UserId,
            Name = $"User {request.UserId}",
            Email = $"user{request.UserId}@example.com"
        };
    }
}
```

### Создание UseCase'а без возвращаемого значения

```csharp
// Запрос
public class UpdateUserRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// UseCase без возвращаемого значения - также чистый, без зависимостей
public class UpdateUser : UseCase<UpdateUserRequest>
{
    protected override async Task<ValueTuple> ExecuteCore(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        // Только бизнес-логика
        Console.WriteLine($"Updating user {request.UserId} with name {request.Name}");
        return default;
    }
}
```

### Использование UseCase'ов

```csharp
// Получаем UseCase напрямую через DI - чистый, без зависимостей
GetUser getUser = serviceProvider.GetRequiredService<GetUser>();
GetUserResponse result = await getUser.Execute(
    new GetUserRequest { UserId = Guid.NewGuid() });

// Или через интерфейс IUseCase
IUseCase<UpdateUserRequest> updateUser = serviceProvider.GetRequiredService<IUseCase<UpdateUserRequest>>();
await updateUser.Execute(new UpdateUserRequest
{
    UserId = Guid.NewGuid(),
    Name = "New Name"
});
```

### Создание пайплайна

```csharp
// Глобальный пайплайн (применяется ко всем UseCase'ам)
[GlobalPipeline]
public class LoggingBehavior<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(
        TRequest request,
        Func<TRequest, CancellationToken, Task<TResponse>> next,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing UseCase {RequestType}", typeof(TRequest).Name);
        
        try
        {
            TResponse response = await next(request, cancellationToken);
            _logger.LogInformation("Successfully executed UseCase {RequestType}", typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing UseCase {RequestType}", typeof(TRequest).Name);
            throw;
        }
    }
}

// Пайплайн для конкретного UseCase'а
[UseCaseFilter(typeof(GetUser))]
public class GetUserPerformanceBehavior<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> HandleAsync(
        TRequest request, 
        Func<TRequest, CancellationToken, Task<TResponse>> next,
        CancellationToken cancellationToken = default)
    {
        DateTime startTime = DateTime.UtcNow;
        
        try
        {
            return await next(request, cancellationToken);
        }
        finally
        {
            TimeSpan elapsed = DateTime.UtcNow - startTime;
            Console.WriteLine($"Operation took {elapsed.TotalMilliseconds} ms");
        }
    }
}
```

## Архитектура

- `IUseCase<TRequest, TResponse>` - базовый интерфейс для UseCase'ов
- `UseCase<TRequest, TResponse>` - базовый класс для UseCase'ов, скрывает инфраструктурную логику
- `UseCaseContext` - статический контекст для применения пайплайнов
- `IPipeline<TRequest, TResponse>` - интерфейс для пайплайнов
- `ServiceCollectionExtensions` - расширения для регистрации UseCase'ов и пайплайнов

## Лицензия

MIT 
