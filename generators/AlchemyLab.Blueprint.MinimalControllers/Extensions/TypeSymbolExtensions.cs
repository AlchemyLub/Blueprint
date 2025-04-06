namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Extensions;

/// <summary>
/// Методы расширения для <see cref="ITypeSymbol"/>
/// </summary>
internal static class TypeSymbolExtensions
{
    public static bool IsPrimitive(this ITypeSymbol? type) =>
        type?.SpecialType is SpecialType.System_Boolean or
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
            SpecialType.System_UInt64;
}
