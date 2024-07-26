namespace AlchemyLub.Blueprint.Generator.Models;

public record struct MethodModel(
    string FullName,
    string ReturnType,
    IReadOnlyCollection<ParameterModel> Parameters,
    string? Keywords = null);
