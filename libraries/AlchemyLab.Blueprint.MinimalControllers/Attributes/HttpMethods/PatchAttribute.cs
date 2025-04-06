namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.HttpMethods;

/// <summary>
/// Определяет метод как обработчик HTTP PATCH запросов
/// </summary>
public sealed class PatchAttribute : HttpMethodAttribute
{
    /// <summary>
    /// Создает новый экземпляр <see cref="PatchAttribute"/>
    /// </summary>
    /// <param name="route">Относительный путь (добавляется к базовому пути контроллера)</param>
    public PatchAttribute(string route = "") : base(HttpMethod.PATCH, route)
    {
    }
}
