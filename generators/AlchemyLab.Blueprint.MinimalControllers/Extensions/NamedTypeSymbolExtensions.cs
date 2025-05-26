namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Extensions;

/// <summary>
/// Методы расширения для <see cref="INamedTypeSymbol"/>
/// </summary>
internal static class NamedTypeSymbolExtensions
{
    private static readonly Dictionary<string, string> AttributeToMethodMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "HttpGetAttribute", "MapGet" },
        { "HttpPostAttribute", "MapPost" },
        { "HttpPutAttribute", "MapPut" },
        { "HttpPatchAttribute", "MapPatch" },
        { "HttpDeleteAttribute", "MapDelete" },
        { "HttpHeadAttribute", "MapHead" },
        { "HttpOptionsAttribute", "MapOptions" }
    };

    /// <summary>
    /// Возвращает название MinimalApi HTTP-метода для данного символа метода контроллера
    /// </summary>
    /// <param name="symbol"><see cref="INamedTypeSymbol"/></param>
    public static string GetMapHttpMethod(this INamedTypeSymbol? symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);

        return AttributeToMethodMap[symbol.Name];
    }
}
