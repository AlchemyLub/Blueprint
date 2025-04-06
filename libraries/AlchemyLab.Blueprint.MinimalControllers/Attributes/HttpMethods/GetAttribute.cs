namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP GET запросов
/// </summary>
public sealed class GetAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="GetAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public GetAttribute(string route = "") : base(HttpMethod.GET, route)
    {
    }
}
