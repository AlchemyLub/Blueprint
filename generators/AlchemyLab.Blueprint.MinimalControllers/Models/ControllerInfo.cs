namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о контроллере
/// </summary>
/// <param name="Name">Имя контроллера.</param>
/// <param name="RouteInfo">Информация о маршруте контроллера</param>
/// <param name="Endpoints">Список эндпоинтов контроллера</param>
/// <param name="Tags">Теги контроллера</param>
/// <param name="SupportedVersions">Поддерживаемые версии</param>
/// <param name="Authorization">Информация об авторизации</param>
/// <param name="Description">Описание контроллера</param>
/// <param name="IsDeprecated">Флаг устаревания контроллера</param>
internal readonly record struct ControllerInfo(
    string Name,
    RouteInfo RouteInfo,
    ImmutableArray<EndpointInfo> Endpoints,
    string[] Tags,
    VersionInfo[]? SupportedVersions = null,
    AuthInfo? Authorization = null,
    string Description = "",
    bool IsDeprecated = false)
{
    /// <summary>
    /// Создаёт новый экземпляр <see cref="ControllerInfo"/> на основе данных контроллера
    /// </summary>
    /// <param name="controllerClassSymbol"><see cref="INamedTypeSymbol"/></param>
    /// <param name="endpoints">Список эндпоинтов контроллера</param>
    /// <param name="routeAttributeData">Информация об атрибуте маршрута</param>
    /// <param name="versionAttributesData">Поддерживаемые версии</param>
    /// <param name="authAttributeData">Информация об атрибуте авторизации</param>
    /// <param name="description">Описание контроллера</param>
    /// <param name="tags">Теги контроллера</param>
    /// <param name="isDeprecated">Флаг устаревания контроллера</param>
    internal static ControllerInfo New(
        INamedTypeSymbol controllerClassSymbol,
        IEnumerable<EndpointInfo> endpoints,
        AttributeData? routeAttributeData,
        AttributeData[]? versionAttributesData,
        AttributeData? authAttributeData,
        string description,
        string[] tags,
        bool isDeprecated) =>
        new(
            controllerClassSymbol.Name,
            routeAttributeData is not null
                ? RouteInfo.New(routeAttributeData)
                : RouteInfo.Empty(),
            [.. endpoints],
            tags,
            versionAttributesData?
                .Select(VersionInfo.NewFromApiVersionAttributeData)
                .ToArray(),
            authAttributeData is not null
                ? AuthInfo.New(authAttributeData)
                : null,
            description,
            isDeprecated);
}
