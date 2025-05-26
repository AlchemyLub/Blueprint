namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Analyzers;

/// <summary>
/// Анализатор контроллеров
/// </summary>
internal static class ControllersAnalyzer
{
    /// <summary>
    /// Получает информацию о контроллере
    /// </summary>
    /// <param name="context"><see cref="GeneratorAttributeSyntaxContext"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Информация о контроллерах</returns>
    public static ControllerInfo? GetControllerInfos(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken = default)
    {
        if (context.TargetNode is not ClassDeclarationSyntax classDeclaration)
        {
            return null;
        }

        Compilation compilation = context.SemanticModel.Compilation;

        if (context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken) is not { } controllerClassSymbol)
        {
            return null;
        }

        if (!HasMinimalControllerAttribute(compilation, controllerClassSymbol))
        {
            return null;
        }

        IEnumerable<EndpointInfo> endpoints = EndpointsAnalyzer.GetEndpointInfos(compilation, controllerClassSymbol);

        AttributeData? routeAttributeData = GetRouteAttributeData(compilation, controllerClassSymbol);

        if (routeAttributeData is null)
        {
            return null;
        }

        bool isDeprecated = HasObsoleteAttribute(compilation, controllerClassSymbol);

        string description = GetDescription(controllerClassSymbol, cancellationToken);

        string[] tags = GetTags(compilation, controllerClassSymbol);

        AttributeData? authAttributeData = GetAuthAttributeData(compilation, controllerClassSymbol);

        return ControllerInfo.New(
            controllerClassSymbol,
            endpoints,
            routeAttributeData,
            authAttributeData,
            description,
            tags,
            isDeprecated);
    }

    private static bool HasMinimalControllerAttribute(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? apiControllerAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.MinimalControllerAttributeName);

        if (apiControllerAttributeSymbol is null)
        {
            return false;
        }

        AttributeData? apiControllerAttribute = classSymbol.GetAttributeDataOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, apiControllerAttributeSymbol));

        return apiControllerAttribute is not null;
    }

    private static bool HasObsoleteAttribute(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? obsoleteAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.ObsoleteAttributeName);

        if (obsoleteAttributeSymbol is null)
        {
            return false;
        }

        AttributeData? obsoleteAttribute = classSymbol.GetAttributeDataOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, obsoleteAttributeSymbol));

        return obsoleteAttribute is not null;
    }

    private static AttributeData? GetRouteAttributeData(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? routeAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.RouteAttributeName);

        return classSymbol.GetAttributeDataOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, routeAttributeSymbol));
    }

    private static AttributeData[] GetVersionAttributesData(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? versionAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.VersionAttributeName);

        return classSymbol.GetAttributeDataOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, versionAttributeSymbol));
    }

    private static string[] GetTags(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? tagAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.TagsAttributeName);

        AttributeData? tagsAttributeSymbol = classSymbol.GetAttributeDataOrDefault(attr =>
            SymbolEqualityComparer.Default.Equals(attr.AttributeClass, tagAttributeSymbol));

        if (tagsAttributeSymbol?.ConstructorArguments.Length is not 1)
        {
            return [];
        }

        return tagsAttributeSymbol.ConstructorArguments[0].Value switch
        {
            ImmutableArray<string> tags => tags.ToArray(),
            string[] tags => tags,
            _ => throw new InvalidOperationException(
                $"Unexpected type of tags in {AttributeNames.TagsAttributeName} attribute. Expected string[] or ImmutableArray<string>, but got {tagsAttributeSymbol.ConstructorArguments[0].Value?.GetType().Name}.")
        };
    }

    private static string GetDescription(
        INamedTypeSymbol classSymbol,
        CancellationToken cancellationToken = default)
    {
        string? documentationComment = classSymbol.GetDocumentationCommentXml(cancellationToken: cancellationToken);

        return string.IsNullOrEmpty(documentationComment)
            ? string.Empty
            : XmlDocParser.GetSummary(documentationComment);
    }

    private static AttributeData? GetAuthAttributeData(
        Compilation compilation,
        INamedTypeSymbol classSymbol)
    {
        INamedTypeSymbol? authAttributeSymbol = compilation.GetTypeByMetadataName(AttributeNames.AuthAttributeName);

        return authAttributeSymbol is not null
            ? classSymbol.GetAttributeDataOrDefault(attr =>
                SymbolEqualityComparer.Default.Equals(attr.AttributeClass, authAttributeSymbol))
            : null;
    }
}
