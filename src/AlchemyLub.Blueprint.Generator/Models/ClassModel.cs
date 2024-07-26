namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ClassModel(
    string Name,
    IReadOnlyCollection<FieldModel> Fields,
    IReadOnlyCollection<ConstantModel> Constants,
    ConstructorModel Constructor,
    IReadOnlyCollection<MethodModel> Methods,
    string? BaseClass = null,
    string? Keywords = null);
