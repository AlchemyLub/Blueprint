namespace AlchemyLab.Blueprint.Application.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями
/// </summary>
public interface IEntityRepository
{
    /// <summary>
    /// Получить сущность из БД
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Запрашиваемая сущность</returns>
    public Task<Entity> GetEntity(Guid id);

    /// <summary>
    /// Создать новую сущность в БД
    /// </summary>
    /// <returns>Идентификатор созданной сущности</returns>
    public Task<Guid> CreateEntity();

    /// <summary>
    /// Удалить сущность из БД
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns><see langword="true"/> если удаление успешно, <see langword="false"/> если нет</returns>
    public Task DeleteEntity(Guid id);

    /// <summary>
    /// Изменить сущность
    /// </summary>
    /// <param name="entity">Новая модель сущности</param>
    /// <returns>Обновлённая модель <see cref="Entity"/></returns>
    public Task<Entity> UpdateEntity(Entity entity);
}
