namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Analyzers;

/// <summary>
/// Анализатор эндпоинтов контроллера
/// </summary>
internal static class EndpointsAnalyzer
{
    /// <summary>
    /// Получает информацию об эндпоинтах контроллера
    /// </summary>
    /// <param name="compilation"><see cref="Compilation"/></param>
    /// <param name="classSymbol">Символ класса контроллера</param>
    /// <returns>Информация об эндпоинтах</returns>
    public static IEnumerable<EndpointInfo> GetEndpointInfos(Compilation compilation, INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? httpMethodAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.HttpMethodAttributeName);
        INamedTypeSymbol? authAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.AuthAttributeName);
        INamedTypeSymbol? obsoleteAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.ObsoleteAttributeName);

        if (httpMethodAttributeSymbol is null)
        {
            yield break;
        }

        foreach (ISymbol member in classSymbol.GetMembers())
        {
            if (member is not IMethodSymbol { MethodKind: MethodKind.Ordinary } methodSymbol)
            {
                continue;
            }

            if (methodSymbol.DeclaredAccessibility is not Accessibility.Public)
            {
                continue;
            }

            AttributeData? httpAttributeData = methodSymbol.GetAttributeDataOrDefault(attr =>
                attr.AttributeClass?.BaseType is not null
                && SymbolEqualityComparer.Default.Equals(attr.AttributeClass.BaseType, httpMethodAttributeSymbol));

            if (httpAttributeData is null)
            {
                continue;
            }

            string description = GetDescription(methodSymbol);

            AttributeData? authAttributeData = authAttributeSymbol is not null
                ? methodSymbol.GetAttributeDataOrDefault(attr =>
                    SymbolEqualityComparer.Default.Equals(attr.AttributeClass, authAttributeSymbol))
                : null;

            AttributeData? obsoleteAttribute = methodSymbol.GetAttributeDataOrDefault(attr =>
                SymbolEqualityComparer.Default.Equals(attr.AttributeClass, obsoleteAttributeSymbol));

            yield return EndpointInfo.New(
                methodSymbol,
                httpAttributeData,
                authAttributeData,
                description,
                obsoleteAttribute is not null);
        }
    }

    private static string GetDescription(
        IMethodSymbol methodSymbol,
        CancellationToken cancellationToken = default)
    {
        string documentationComment = methodSymbol.GetDocumentationCommentXml(cancellationToken: cancellationToken) ?? string.Empty;
        return XmlDocParser.GetSummary(documentationComment);
    }
}
