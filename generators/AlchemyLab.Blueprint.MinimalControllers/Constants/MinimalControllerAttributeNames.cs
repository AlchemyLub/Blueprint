namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Constants;

/// <summary>
/// Имена атрибутов, используемых в контроллерах.
/// </summary>
internal static class MinimalControllerAttributeNames
{
    private const string BaseName = $"{nameof(AlchemyLab)}.{nameof(Blueprint)}.{nameof(MinimalControllers)}.{nameof(Attributes)}";

    /// <summary>
    /// Имя атрибута контроллера
    /// </summary>
    internal const string ApiControllerAttributeName = $"{BaseName}.ApiControllerAttribute";

    /// <summary>
    /// Базовое имя атрибута метода контроллера
    /// </summary>
    internal const string HttpMethodAttributeBaseName = $"{BaseName}.HttpMethodAttribute";

    /// <summary>
    /// Имя атрибута авторизации контроллера
    /// </summary>
    internal const string AuthAttributeBaseName = $"{BaseName}.MinimalAuthAttribute";
}
