namespace FloralHub.Blueprint.Application.Services.Abstractions;

/// <summary>
/// Сервис инфраструктурного слоя
/// </summary>
public interface IInfrastructureService
{
    /// <summary>
    /// Получить сущность из БД
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Запрашиваемая сущность</returns>
    public Task<IEntity> GetDbEntityAsync(Guid id);
}
