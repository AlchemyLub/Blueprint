namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ConstructorModel(
    string ClassName,
    IReadOnlyCollection<ParameterModel> Parameters,
    IReadOnlyCollection<ConstructorFieldModel> Fields);
