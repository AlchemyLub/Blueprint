namespace AlchemyLab.Blueprint.UseCase.Examples.UseCases;

/// <summary>
/// Запрос для получения пользователя
/// </summary>
public class GetUserRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Ответ с данными пользователя
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Пример UseCase'а для получения пользователя
/// </summary>
public class GetUser : UseCase<GetUserRequest, GetUserResponse>
{
    /// <summary>
    /// Конструктор юзкейса
    /// </summary>
    /// <param name="context">Контекст юзкейса</param>
    public GetUser(UseCaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override async Task<GetUserResponse> ExecuteCore(GetUserRequest request)
    {
        // В реальном проекте здесь будет логика получения пользователя из базы данных или другого источника данных
        await Task.Delay(100); // Имитация задержки

        // Пример простой логики
        return new GetUserResponse
        {
            Id = request.UserId,
            Name = $"User {request.UserId}",
            Email = $"user{request.UserId}@example.com"
        };
    }
}
