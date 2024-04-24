namespace AlchemyLub.Blueprint.Endpoints.Controllers;

/// <summary>
/// Шаблонный контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BlueprintController(IInfrastructureService infrastructureService) : ControllerBase
{
    /// <summary>
    /// Шаблонный endpoint.
    /// </summary>
    [HttpGet("test")]
    public async Task<IEntity> Get() => await infrastructureService.GetDbEntityAsync(Guid.NewGuid());
}
