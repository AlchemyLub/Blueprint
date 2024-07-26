namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ConstantModel(string TypeName, string Name, string Value)
{
    public string ToFullString() => $"private const {TypeName} {Name} = {Value};";

    public override string ToString() => ToFullString();
}
