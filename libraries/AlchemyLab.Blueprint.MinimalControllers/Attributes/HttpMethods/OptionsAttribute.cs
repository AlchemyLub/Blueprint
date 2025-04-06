namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP OPTIONS запросов
/// </summary>
public sealed class OptionsAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="HeadAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public OptionsAttribute(string route = "") : base(HttpMethod.OPTIONS, route)
    {
    }
}
