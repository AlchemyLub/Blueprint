namespace AlchemyLab.Blueprint.Application.Services.Abstractions;

/// <summary>
/// Сервис слоя приложения
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Запрашиваемая сущность</returns>
    public Task<Entity> GetEntity(Guid id);

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    /// <returns>Идентификатор новой сущности</returns>
    public Task<Guid> CreateEntity();

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns><see langword="true"/> если удаление успешно, <see langword="false"/> если нет</returns>
    public Task<Result> DeleteEntity(Guid id);

    /// <summary>
    /// Изменить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="request">Модель запроса</param>
    /// <returns>Обновлённая модель <see cref="Entity"/></returns>
    public Task<Entity> UpdateEntity(Guid id, Entity request);
}
