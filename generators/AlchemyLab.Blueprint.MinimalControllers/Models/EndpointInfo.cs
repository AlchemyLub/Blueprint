namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о методе контроллера
/// </summary>
/// <param name="MethodName">Имя метода</param>
/// <param name="HttpMethod">HTTP-метод (MapGet, MapPost, и т.д.)</param>
/// <param name="Route">Полный маршрут эндпоинта</param>
/// <param name="ReturnType">Тип возвращаемого значения</param>
/// <param name="Parameters">Параметры метода</param>
/// <param name="HasBodyParameter">Наличие параметра из тела запроса</param>
/// <param name="HasRouteParameters">Наличие параметров маршрута</param>
/// <param name="IsAsync">Является ли метод асинхронным</param>
/// <param name="RequiresAuth">Требуется ли авторизация</param>
/// <param name="Description">Описание метода</param>
internal readonly record struct EndpointInfo(
    string MethodName,
    string HttpMethod,
    string Route,
    string ReturnType,
    ImmutableArray<ParameterInfo> Parameters,
    bool HasBodyParameter,
    bool HasRouteParameters,
    bool IsAsync,
    bool RequiresAuth,
    string Description = "");
