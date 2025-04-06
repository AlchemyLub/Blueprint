namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods.Core;

/// <summary>
/// Базовый класс для атрибутов HTTP-методов
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public abstract class HttpMethodAttribute : Attribute
{
    /// <summary>
    /// HTTP метод
    /// </summary>
    public required HttpMethod Method { get; init; }

    /// <summary>
    /// Относительный путь для эндпоинта (добавляется к базовому пути контроллера)
    /// </summary>
    public required string Route { get; init; }

    /// <summary>
    /// Создает новый экземпляр <see cref="HttpMethodAttribute"/>
    /// </summary>
    /// <param name="method">HTTP метод</param>
    /// <param name="route">Относительный путь</param>
    protected HttpMethodAttribute(HttpMethod method, string route = "")
    {
        Method = method;
        Route = route;
    }
}
