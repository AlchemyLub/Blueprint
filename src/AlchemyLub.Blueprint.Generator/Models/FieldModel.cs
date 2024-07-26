namespace AlchemyLub.Blueprint.Generator.Models;

public record struct FieldModel(string TypeName, string Name)
{
    public string ToFullString() => $"private readonly {TypeName} {Name};";

    public override string ToString() => ToFullString();
}
