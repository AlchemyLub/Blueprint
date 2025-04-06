namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Extensions;

/// <summary>
/// Методы расширения для <see cref="INamedTypeSymbol"/>
/// </summary>
internal static class NamedTypeSymbolExtensions
{
    private static readonly Dictionary<string, string> AttributeToMethodMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { nameof(GetAttribute), "MapGet" },
        { nameof(PostAttribute), "MapPost" },
        { nameof(PutAttribute), "MapPut" },
        { nameof(PatchAttribute), "MapPatch" },
        { nameof(DeleteAttribute), "MapDelete" },
        { nameof(HeadAttribute), "MapHead" },
        { nameof(OptionsAttribute), "MapOptions" }
    };

    public static string? GetHttpMethod(this INamedTypeSymbol? symbol) =>
        symbol is not null
            ? AttributeToMethodMap.GetValueOrDefault(symbol.Name)
            : null;
}
