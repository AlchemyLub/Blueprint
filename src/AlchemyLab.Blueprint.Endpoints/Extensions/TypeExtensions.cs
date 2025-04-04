namespace AlchemyLab.Blueprint.Endpoints.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether a type is a custom type,
    /// i.e., a type that is not a primitive type, enum, or a type from the <see cref="System"/> namespace.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><see langword="true"/> if the type is a custom type; otherwise, <see langword="false"/>.</returns>
    public static bool IsCustomType(this Type? type) =>
        type is { IsPrimitive: false, IsEnum: false }
        && (!type.Namespace?.StartsWith("System") ?? false)
        && !type.IsAssignableFrom(typeof(object))
        && type.Assembly != typeof(object).Assembly;
}
