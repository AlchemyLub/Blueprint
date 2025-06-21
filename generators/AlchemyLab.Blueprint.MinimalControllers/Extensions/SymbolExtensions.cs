namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Extensions;

/// <summary>
/// Методы расширения для <see cref="ITypeSymbol"/>
/// </summary>
internal static class SymbolExtensions
{
    // Список имен типов, которые считаются примитивными
    private static readonly HashSet<string> PrimitiveTypeNames =
    [
        "System.Boolean",
        "System.Byte",
        "System.SByte",
        "System.Int16",
        "System.UInt16",
        "System.Int32",
        "System.UInt32",
        "System.Int64",
        "System.UInt64",
        "System.IntPtr",
        "System.UIntPtr",
        "System.Char",
        "System.Double",
        "System.Single",
        "System.Decimal",
        "System.String",
        "System.DateTime",
        "System.DateTimeOffset",
        "System.TimeSpan",
        "System.Guid",
        "System.Uri"
    ];

    /// <summary>
    /// Проверяет, является ли тип примитивным или строкой
    /// </summary>
    /// <param name="typeSymbol">Символ типа</param>
    /// <returns><see langword="true"/>, если тип примитивный или строка, иначе <see langword="false"/></returns>
    public static bool IsPrimitive(this ITypeSymbol typeSymbol)
    {
        ArgumentNullException.ThrowIfNull(typeSymbol);

        // Проверяем встроенные примитивные типы и строки
        if (typeSymbol.SpecialType is
            SpecialType.System_Boolean or
            SpecialType.System_Byte or
            SpecialType.System_Char or
            SpecialType.System_DateTime or
            SpecialType.System_Decimal or
            SpecialType.System_Double or
            SpecialType.System_Int16 or
            SpecialType.System_Int32 or
            SpecialType.System_Int64 or
            SpecialType.System_SByte or
            SpecialType.System_Single or
            SpecialType.System_String or
            SpecialType.System_UInt16 or
            SpecialType.System_UInt32 or
            SpecialType.System_UInt64)
        {
            return true;
        }

        // Проверяем на Guid
        if (PrimitiveTypeNames.Contains(typeSymbol.ToDisplayString()))
        {
            return true;
        }

        if (typeSymbol.TypeKind is TypeKind.Enum)
        {
            return true;
        }

        if (typeSymbol is INamedTypeSymbol { IsGenericType: true } namedType &&
            namedType.ConstructedFrom.ToDisplayString() is "System.Nullable<T>")
        {
            return namedType.TypeArguments.FirstOrDefault()?.IsPrimitive() ?? false;
        }

        return false;
    }

    /// <summary>
    /// Возвращает аттрибут, которым помечен символ, если найден, иначе <see langword="null"/>
    /// </summary>
    /// <param name="symbol"><see cref="ISymbol"/></param>
    /// <param name="predicate">Предикат для поиска данных об атрибуте</param>
    public static AttributeData? GetAttributeDataOrDefault(this ISymbol? symbol, Func<AttributeData, bool> predicate) =>
        symbol?.GetAttributes().FirstOrDefault(predicate);

    /// <summary>
    /// Возвращает коллекцию аттрибутов, которыми помечен символ
    /// </summary>
    /// <param name="symbol"><see cref="ISymbol"/></param>
    /// <param name="predicate">Предикат для поиска данных об атрибуте</param>
    public static IEnumerable<AttributeData>? WhereAttribute(this ISymbol? symbol, Func<AttributeData, bool> predicate) =>
        symbol?.GetAttributes().Where(predicate);
}
