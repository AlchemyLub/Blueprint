namespace AlchemyLub.Blueprint.SharedKernel.Attributes;

// TODO: Возможно стоит унести такие вещи в другой слой.
/// <summary>
/// Атрибут для игнорирования конкретного значения <see langword="enum"/> в OpenApi документации
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class OpenApiIgnoreEnumAttribute : Attribute
{
}
