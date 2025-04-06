namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP POST запросов
/// </summary>
public sealed class PostAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="PostAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public PostAttribute(string route = "") : base(HttpMethod.POST, route)
    {
    }
}
