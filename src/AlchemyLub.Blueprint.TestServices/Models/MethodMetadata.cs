namespace AlchemyLub.Blueprint.TestServices.Models;

/// <summary>
/// Метаданные метода
/// </summary>
public readonly struct MethodMetadata(string methodName, MethodParameter[] parameters, Type returnType)
{
    /// <summary>
    /// Название метода
    /// </summary>
    public string Name { get; } = methodName;

    /// <summary>
    /// Параметры метода
    /// </summary>
    public MethodParameter[] Parameters { get; } = parameters;

    /// <summary>
    /// Возвращаемый тип метода
    /// </summary>
    public Type ReturnType { get; } = returnType;
}
