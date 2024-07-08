namespace AlchemyLub.Blueprint.Generator.Models;

public record struct DecoratedClassModel(
    string InterfaceName,
    IReadOnlyCollection<MethodModel> PublicMethods,
    IReadOnlyCollection<string> DecoratorNames);
