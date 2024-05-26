namespace AlchemyLub.Blueprint.TestServices.Models;

/// <summary>
/// Значение перечисления [<see langword="enum"/>]
/// </summary>
public readonly struct EnumField(int value, string name)
{
    /// <summary>
    /// Значение перечисления
    /// </summary>
    public int Value { get; } = value;

    /// <summary>
    /// Наименование значения перечисления
    /// </summary>
    public string Name { get; } = name;
}
