namespace AlchemyLub.Blueprint.TestServices.Models;

/// <summary>
/// Представляет собой тип класса-контракта(контроллера или клиента)
/// </summary>
public readonly struct ContractType(string name, string shortName, MethodMetadata[] methods)
{
    /// <summary>
    /// Название класса-контракта
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Сокращённое название класса-контракта
    /// </summary>
    public string ShortName { get; } = shortName;

    /// <summary>
    /// Методы класса-контракта
    /// </summary>
    public MethodMetadata[] Methods { get; } = methods;
}
