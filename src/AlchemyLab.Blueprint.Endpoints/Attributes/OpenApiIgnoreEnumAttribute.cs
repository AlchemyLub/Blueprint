namespace AlchemyLab.Blueprint.Endpoints.Attributes;

/// <summary>
/// Атрибут для игнорирования конкретного значения <see langword="enum"/> в OpenApi документации
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class OpenApiIgnoreEnumAttribute : Attribute
{
}
