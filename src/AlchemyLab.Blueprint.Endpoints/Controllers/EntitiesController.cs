namespace AlchemyLab.Blueprint.Endpoints.Controllers;

/// <summary>
/// Базовый контроллер сущностей.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntitiesController(
    IApplicationService applicationService,
    ILogger<EntitiesController> logger)
    : ControllerBase
{
    /// <summary>
    /// Получить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [HttpGet("{id}")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<EntityResponse> GetEntity(Guid id)
    {
        logger.LogTrace("Beginning of getting an entity with id [{Id}]", id);

        Entity entity = await applicationService.GetEntity(id);

        logger.LogInformation("Completed getting entity with id [{Id}]", id);

        return new(entity.Id, entity.Title);
    }

    /// <summary>
    /// Создать сущность
    /// </summary>
    [HttpPost]
    public async Task<Guid> CreateEntity()
    {
        logger.LogTrace("Beginning of creating an entity");

        Guid entityId = await applicationService.CreateEntity();

        logger.LogInformation("Completed creating entity. ID of the new entity [{Id}]", entityId);

        return entityId;
    }

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [HttpDelete("{id}")]
    public async Task<NoContent> DeleteEntity(Guid id)
    {
        await applicationService.DeleteEntity(id);

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="requests">Новая сущность</param>
    [HttpPatch("{id}")]
    public async Task<EntityResponse> UpdateEntity(Guid id, EntityRequest requests)
    {
        logger.LogTrace("Beginning of updating an entity with id [{Id}]", id);

        Entity newEntity = new(id)
        {
            Title = requests.Title,
            Description = requests.Description,
            CreatedAt = DateTime.UtcNow
        };

        Entity entity = await applicationService.UpdateEntity(id, newEntity);

        logger.LogInformation("Successfully update an entity with id [{Id}]", id);

        return new(entity.Id, entity.Title);
    }

    /// <summary>
    /// Выбросить исключение
    /// </summary>
    [HttpPatch("throw")]
    public Task WrongEndpoint()
    {
        logger.LogWarning("Start handle wrong endpoint");

        throw new NotImplementedException();
    }
}
