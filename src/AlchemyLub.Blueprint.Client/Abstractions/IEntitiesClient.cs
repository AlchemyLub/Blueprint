namespace AlchemyLub.Blueprint.Client.Abstractions;

/// <summary>
/// Клиент базовых сущностей
/// </summary>
public interface IEntitiesClient
{
    private const string BaseEntitiesUrl = "/api/entities";
    private const string BaseEntitiesUrlWithId = BaseEntitiesUrl + "/{id}";

    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [Get(BaseEntitiesUrlWithId)]
    public Task<EntityResponse> GetEntity(Guid id);

    /// <summary>
    /// Создать сущность
    /// </summary>
    [Post(BaseEntitiesUrl)]
    public Task<Guid> CreateEntity();

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [Delete(BaseEntitiesUrlWithId)]
    public Task<bool> DeleteEntity(Guid id);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="request">Новая сущность</param>
    [Patch(BaseEntitiesUrlWithId)]
    public Task<EntityResponse> UpdateEntity(Guid id, EntityRequest request);
}
