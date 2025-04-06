namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP DELETE запросов
/// </summary>
public sealed class DeleteAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="DeleteAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public DeleteAttribute(string route = "") : base(HttpMethod.DELETE, route)
    {
    }
}
