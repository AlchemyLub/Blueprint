namespace AlchemyLab.Blueprint.MinimalControllers.Attributes.Controllers;

/// <summary>
/// Указывает, что класс является API контроллером в стиле MinimalApi
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MinimalControllerAttribute : Attribute
{
    /// <summary>
    /// Базовый путь для всех эндпоинтов контроллера
    /// </summary>
    public required string Route { get; init; }

    /// <summary>
    /// Создает новый экземпляр <see cref="MinimalControllerAttribute"/>
    /// </summary>
    /// <param name="route">Базовый путь для всех эндпоинтов контроллера</param>
    public MinimalControllerAttribute(string route) => Route = route;
}
