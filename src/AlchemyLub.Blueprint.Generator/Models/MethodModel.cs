namespace AlchemyLub.Blueprint.Generator.Models;

public record struct MethodModel(string FullName, IReadOnlyCollection<ParameterModel> Parameters, string ReturnType);
