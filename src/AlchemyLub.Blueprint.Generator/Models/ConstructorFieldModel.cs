namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ConstructorFieldModel(string ClassFieldName, string ParameterFieldName)
{
    public string ToFullString() => $"this.{ClassFieldName} = {ParameterFieldName};";

    public override string ToString() => ToFullString();
}
