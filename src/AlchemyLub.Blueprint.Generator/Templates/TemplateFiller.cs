namespace AlchemyLub.Blueprint.Generator.Templates;

public static class TemplateFiller
{
    public static string GenerateDecorator(
        string interfaceName,
        string serviceName,
        string decoratorName) =>
$$"""
public class {{serviceName}}{{decoratorName}} : {{interfaceName}}
{
    private readonly ITrace trace;
    private readonly {{interfaceName}} wrappedService;

    public {{serviceName}}{{decoratorName}}(ITrace trace, {{interfaceName}} wrappedService)
    {
        this.trace = trace;
        this.wrappedService = wrappedService;
    }

    public async Task<Guid> GetUserName(Guid id)
    {
        using (var span = trace.BuildSpan(nameof(GetUserName)).StartActive(true))
        {
            return await wrappedService.GetUserName(id);
        }
    }
}
""";

    public static string GenerateReadonlyFields(
        string interfaceName,
        IReadOnlyCollection<ParameterModel> decoratorFieldTypes)
    {
        const string privateReadonly = "private readonly";

        return
$$"""
    {{privateReadonly}} {{interfaceName}} wrappedService;
    {{decoratorFieldTypes.Select(fieldTypeName => $"{privateReadonly} {fieldTypeName};")}}
""";
    }

    public static string GenerateConstructor(
        string interfaceName,
        string serviceName,
        string decoratorName,
        IReadOnlyCollection<ParameterModel> decoratorFieldTypes)
    {
        return
$$"""
    public {{serviceName}}{{decoratorName}}(
        {{interfaceName}} wrappedService,
        {{FormatConstructorParameters(decoratorFieldTypes)}})
    {
        this.wrappedService = wrappedService;
        {{FormatFieldsInConstructor(decoratorFieldTypes)}}
    }
""";
    }

    public static string FormatConstructorParameters(IReadOnlyCollection<ParameterModel> decoratorFieldTypes) =>
        string.Join($",{SyntaxFactory.CarriageReturnLineFeed.ToString(),-10}", decoratorFieldTypes);

    public static string FormatFieldsInConstructor(IReadOnlyCollection<ParameterModel> decoratorFieldTypes)
    {
        IEnumerable<string> rawFields = decoratorFieldTypes.Select(t => $"this.{t.Name} = {t.Name};");

        return string.Join($"{SyntaxFactory.CarriageReturnLineFeed.ToString(),-10}", rawFields);
    }
}
