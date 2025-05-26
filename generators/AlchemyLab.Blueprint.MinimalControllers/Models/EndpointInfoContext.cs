namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Контекст информации о контроллере
/// </summary>
public sealed class EndpointInfoContext
{
    public required INamedTypeSymbol HttpMethodAttributeSymbol { get; init; }
    public INamedTypeSymbol? AuthAttributeSymbol { get; init; }

    public static EndpointInfoContext? Create(Compilation compilation)
    {
        INamedTypeSymbol? httpMethodSymbol = compilation.GetTypeByMetadataName(AttributeNames.HttpMethodAttributeName);

        if (httpMethodSymbol is null)
        {
            return null;
        }

        return new()
        {
            HttpMethodAttributeSymbol = httpMethodSymbol,
            AuthAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.AuthAttributeName)
        };
    }
}
