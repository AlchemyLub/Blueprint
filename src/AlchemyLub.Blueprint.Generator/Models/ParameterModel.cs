namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ParameterModel(string TypeName, string Name)
{
    public override string ToString() => $"{TypeName} {Name}";
}
