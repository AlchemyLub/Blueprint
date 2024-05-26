namespace AlchemyLub.Blueprint.TestServices.Models;

/// <summary>
/// Параметр метода
/// </summary>
public readonly struct MethodParameter(Type type, string name)
{
    /// <summary>
    /// Тип параметра
    /// </summary>
    public Type Type { get; } = type;

    /// <summary>
    /// Имя параметра
    /// </summary>
    public string Name { get; } = name;
}
