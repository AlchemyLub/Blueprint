namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Analyzers;

/// <summary>
/// Анализатор кода с возможностью генерации диагностических сообщений
/// </summary>
internal static class CodeAnalyzer
{
    /// <summary>
    /// Проверяет, что публичный метод имеет HTTP-атрибут
    /// </summary>
    /// <param name="methodSymbol">Символ метода</param>
    /// <param name="classSymbol">Символ класса</param>
    /// <returns>Диагностику, если метод не имеет HTTP-атрибута</returns>
    public static Diagnostic? CheckHttpAttributePresent(IMethodSymbol methodSymbol, INamedTypeSymbol classSymbol)
    {
        if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
        {
            return null;
        }

        // Логика проверки наличия HTTP-атрибута
        // Если атрибут отсутствует, вернуть диагностику

        return Diagnostic.Create(
            DiagnosticDescriptors.MissingHttpMethodAttribute,
            methodSymbol.Locations[0],
            methodSymbol.Name,
            classSymbol.Name);
    }

    /// <summary>
    /// Проверяет, что метод с HTTP-атрибутом является публичным
    /// </summary>
    /// <param name="methodSymbol">Символ метода</param>
    /// <param name="classSymbol">Символ класса</param>
    /// <param name="httpAttribute">HTTP-атрибут</param>
    /// <returns>Диагностику, если метод не является публичным</returns>
    public static Diagnostic? CheckMethodIsPublic(IMethodSymbol methodSymbol, INamedTypeSymbol classSymbol, AttributeData httpAttribute)
    {
        if (methodSymbol.DeclaredAccessibility == Accessibility.Public)
        {
            return null;
        }

        return Diagnostic.Create(
            DiagnosticDescriptors.NonPublicHttpMethod,
            methodSymbol.Locations[0],
            methodSymbol.Name,
            classSymbol.Name,
            methodSymbol.DeclaredAccessibility.ToString().ToLowerInvariant());
    }
}
