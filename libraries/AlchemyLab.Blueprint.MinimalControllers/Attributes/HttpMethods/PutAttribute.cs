namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP PUT запросов
/// </summary>
public sealed class PutAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="PutAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public PutAttribute(string route = "") : base(HttpMethod.PUT, route)
    {
    }
}
