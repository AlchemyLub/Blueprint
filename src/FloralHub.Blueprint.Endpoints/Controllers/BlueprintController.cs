namespace FloralHub.Blueprint.Endpoints.Controllers;

/// <summary>
/// Шаблонный контроллер.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BlueprintController : ControllerBase
{
    /// <summary>
    /// Шаблонный endpoint.
    /// </summary>
    [HttpGet("test")]
    public int Get() => int.MaxValue;
}
