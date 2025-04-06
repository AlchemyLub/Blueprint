namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP HEAD запросов
/// </summary>
public sealed class HeadAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="HeadAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public HeadAttribute(string route = "") : base(HttpMethod.HEAD, route)
    {
    }
}
