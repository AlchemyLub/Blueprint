namespace AlchemyLub.Blueprint.Generator.Models;

public record struct ParameterModel(string TypeName, string Name, string? Keywords = null, string? DefaultValue = null)
{
    public string ToFullString() => (Keywords, DefaultValue) switch
    {
        (not null, not null) => $"{Keywords} {TypeName} {Name} = {DefaultValue}",
        (null, not null) => $"{TypeName} {Name} = {DefaultValue}",
        (not null, null) => $"{Keywords} {TypeName} {Name}",
        _ => $"{TypeName} {Name}"
    };

    public override string ToString() => ToFullString();
}
