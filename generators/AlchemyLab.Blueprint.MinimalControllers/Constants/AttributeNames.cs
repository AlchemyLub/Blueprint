namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Constants;

/// <summary>
/// Имена атрибутов, используемых в контроллерах.
/// </summary>
internal static class AttributeNames
{
    private const string ProjectBaseName = "AlchemyLab.Blueprint.MinimalControllers";

    /// <summary>
    /// Имя атрибута контроллера
    /// </summary>
    internal const string MinimalControllerAttributeName = $"{ProjectBaseName}.Attributes.{nameof(MinimalControllerAttribute)}";

    /// <summary>
    /// Базовое имя атрибута метода контроллера
    /// </summary>
    internal const string HttpMethodAttributeName = "Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute";

    /// <summary>
    /// Имя атрибута авторизации контроллера
    /// </summary>
    internal const string AuthAttributeName = "Microsoft.AspNetCore.Authorization.AuthorizeAttribute";

    /// <summary>
    /// Имя атрибута маршрута контроллера
    /// </summary>
    internal const string RouteAttributeName = "Microsoft.AspNetCore.Mvc.RouteAttribute";

    /// <summary>
    /// Имя атрибута тегов контроллера
    /// </summary>
    internal const string TagsAttributeName = "Microsoft.AspNetCore.Http.TagsAttribute";

    /// <summary>
    /// Имя атрибута который помечает метод контроллера как исключаемый из документации
    /// </summary>
    internal const string ExcludeFromDescriptionAttributeName = "Microsoft.AspNetCore.Routing.ExcludeFromDescriptionAttribute";

    /// <summary>
    /// Имя атрибута который помечает метод контроллера как устаревший
    /// </summary>
    internal const string ObsoleteAttributeName = $"System.{nameof(ObsoleteAttribute)}";

    /// <summary>
    /// Имя атрибута используемого для версионирования API
    /// </summary>
    internal const string ApiVersionAttributeName = "Asp.Versioning.ApiVersionAttribute";

    /// <summary>
    /// Имя атрибута используемого для указания активных версий API
    /// </summary>
    internal const string MapToApiVersionAttributeName = "Asp.Versioning.MapToApiVersionAttribute";
}
