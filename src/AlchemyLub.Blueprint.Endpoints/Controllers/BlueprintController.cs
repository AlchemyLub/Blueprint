namespace AlchemyLub.Blueprint.Endpoints.Controllers;

/// <summary>
/// Шаблонный контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BlueprintController(IInfrastructureService infrastructureService) : ControllerBase
{
    /// <summary>
    /// Получить сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    [HttpGet]
    public async Task<IEntity> Get(Guid id) => await infrastructureService.GetDbEntity(id);
}
