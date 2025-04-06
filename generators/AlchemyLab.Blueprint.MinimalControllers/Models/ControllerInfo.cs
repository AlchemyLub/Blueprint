namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о контроллере.
/// </summary>
/// <param name="Name">Имя контроллера.</param>
/// <param name="Namespace">Пространство имен контроллера.</param>
/// <param name="Route">Базовый маршрут контроллера.</param>
/// <param name="Endpoints">Список эндпоинтов контроллера.</param>
/// <param name="DependencyTypes">Типы зависимостей контроллера.</param>
/// <param name="Description">Описание контроллера</param>
internal readonly record struct ControllerInfo(
    string Name,
    string Namespace,
    string Route,
    ImmutableArray<EndpointInfo> Endpoints,
    ImmutableArray<string> DependencyTypes,
    string Description = "");
