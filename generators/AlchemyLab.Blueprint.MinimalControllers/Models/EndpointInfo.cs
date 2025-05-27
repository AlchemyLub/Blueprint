namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о методе контроллера
/// </summary>
/// <param name="MethodFullName">Полное имя метода</param>
/// <param name="HttpMethod">HTTP-метод (MapGet, MapPost, и т.д.)</param>
/// <param name="RouteInfo">Информация о маршруте эндпоинта</param>
/// <param name="ActiveVersions">Версии, в которых данный endpoint актуален и работает</param>
/// <param name="Authorization">Информация об авторизации</param>
/// <param name="Description">Описание метода</param>
/// <param name="IsDeprecated">Флаг устаревания метода</param>
internal record struct EndpointInfo(
    string MethodFullName,
    string HttpMethod,
    RouteInfo RouteInfo,
    VersionInfo[]? ActiveVersions = null,
    AuthInfo? Authorization = null,
    string Description = "",
    bool IsDeprecated = false)
{
    public string? ApiVersionSetName { get; private set; }

    public int? CurrentVersion { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="EndpointInfo"/> на основе данных метода контроллера
    /// </summary>
    /// <param name="endpointMethodSymbol"><see cref="IMethodSymbol"/></param>
    /// <param name="httpAttributeData">Информация об атрибуте HTTP-метода</param>
    /// <param name="authAttributeData">Информация об атрибуте авторизации</param>
    /// <param name="description">Описание метода</param>
    /// <param name="isDeprecated">Флаг устаревания метода</param>
    internal static EndpointInfo New(
        IMethodSymbol endpointMethodSymbol,
        AttributeData httpAttributeData,
        AttributeData? authAttributeData,
        string description,
        bool isDeprecated)
    {
        ArgumentNullException.ThrowIfNull(httpAttributeData.AttributeClass);

        return new(
            GetFullMethodPath(endpointMethodSymbol),
            httpAttributeData.AttributeClass.GetMapHttpMethod(),
            RouteInfo.New(httpAttributeData),
            authAttributeData is not null ? AuthInfo.New(authAttributeData) : null,
            description,
            isDeprecated);
    }

    /// <summary>
    /// Устанавливает имя набора версий API и текущую версию для эндпоинта
    /// </summary>
    /// <param name="apiVersionSetName">Имя набора версий API</param>
    /// <param name="currentVersion">Текущая версия эндпоинта</param>
    internal void SetVersion(string apiVersionSetName, int currentVersion)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiVersionSetName);
        ArgumentNullException.ThrowIfNull(currentVersion);

        ApiVersionSetName = apiVersionSetName;
        CurrentVersion = currentVersion;
    }

    private static string GetFullMethodPath(IMethodSymbol methodSymbol)
    {
        string containingType = methodSymbol.ContainingType.ToDisplayString();
        string methodName = methodSymbol.Name;
        return $"{containingType}.{methodName}";
    }
}
