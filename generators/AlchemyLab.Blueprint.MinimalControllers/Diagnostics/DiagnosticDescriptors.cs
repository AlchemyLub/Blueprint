namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Diagnostics;

/// <summary>
/// Содержит дескрипторы диагностик для генератора MinimalControllers.
/// </summary>
internal static class DiagnosticDescriptors
{
    public const string Category = "MinimalControllers";

    /// <summary>
    /// Диагностика для случаев, когда публичный метод не помечен HTTP-атрибутом.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingHttpMethodAttribute = new(
        "MC001",
        "Missing HttpMethodAttribute",
        "Публичный метод {0} в контроллере {1} должен быть помечен атрибутом HTTP (например, [HttpGet], [HttpPost] и т.д.).",
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        helpLinkUri: "https://docs.minimalcontrollers.example/missing-http-attribute");

    /// <summary>
    /// Диагностика для случаев, когда метод с HTTP-атрибутом имеет не-public модификатор доступа.
    /// </summary>
    public static readonly DiagnosticDescriptor NonPublicHttpMethod = new(
        "MC002",
        "NonPublic HttpMethod",
        "Метод {0} в контроллере {1} помечен HTTP-атрибутом, но имеет модификатор доступа {2}. Должен быть public.",
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: "https://docs.minimalcontrollers.example/non-public-http-method");
}
