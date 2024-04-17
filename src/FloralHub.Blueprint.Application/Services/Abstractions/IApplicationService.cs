namespace FloralHub.Blueprint.Application.Services.Abstractions;

/// <summary>
/// Сервис слоя приложения
/// </summary>
internal interface IApplicationService
{
    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Запрашиваемая сущность</returns>
    public Task<IEntity> GetEntityAsync(Guid id);
}
