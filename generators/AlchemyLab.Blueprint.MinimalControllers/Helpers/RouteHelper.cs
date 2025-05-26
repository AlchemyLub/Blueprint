namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Helpers;

/// <summary>
/// Методы для работы с маршрутами API
/// </summary>
public static class RouteHelper
{
    /// <summary>
    /// Объединяет базовый маршрут и маршрут эндпоинта
    /// </summary>
    /// <param name="baseRoute">Базовый маршрут</param>
    /// <param name="endpointRoute">Маршрут эндпоинта</param>
    /// <returns>Объединенный маршрут</returns>
    public static string CombineRoutes(string baseRoute, string endpointRoute)
    {
        if (string.IsNullOrEmpty(endpointRoute))
        {
            return baseRoute;
        }

        // Убираем лишние слэши на стыке путей
        if (baseRoute.EndsWith("/"))
        {
            baseRoute = baseRoute.TrimEnd('/');
        }

        if (endpointRoute.StartsWith("/"))
        {
            endpointRoute = endpointRoute.TrimStart('/');
        }

        return $"{baseRoute}/{endpointRoute}";
    }

    /// <summary>
    /// Обрезает постфикс "Controller" из имени контроллера, если он есть
    /// </summary>
    /// <param name="controllerName">Имя контроллера</param>
    public static string GetControllerNameWithoutPostfix(string controllerName)
    {
        const string controllerSuffix = "Controller";

        return controllerName.EndsWith(controllerSuffix)
            ? controllerName[..^controllerSuffix.Length]
            : controllerName;
    }

    /// <summary>
    /// Преобразует первый символ строки в нижний регистр
    /// </summary>
    /// <param name="sourceString">Исходная строка</param>
    public static string LowerFirstLetter(string sourceString)
    {
        if (string.IsNullOrEmpty(sourceString))
        {
            return string.Empty;
        }

        return char.ToLowerInvariant(sourceString[0]) + sourceString[1..];
    }
}
