namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ClassModel(
    string Name,
    IReadOnlyCollection<FieldModel> Fields,
    IReadOnlyCollection<ConstantModel> Constants,
    IReadOnlyCollection<MethodModel> Methods,
    IReadOnlyCollection<BaseClassModel> BaseClasses,
    ConstructorModel? Constructor = null);
