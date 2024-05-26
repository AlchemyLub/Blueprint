namespace AlchemyLub.Blueprint.Endpoints.SchemaFilters;

/// <summary>
/// Фильтр позволяющий не отображать в Swagger значения перечислений [<see langword="enum"/>], помеченных атрибутом
/// </summary>
public sealed class OpenApiIgnoreEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumOpenApiStrings = new List<IOpenApiAny>();

            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                var member = context.Type.GetMember(enumValue.ToString() ?? string.Empty)[0];
                if (!member.GetCustomAttributes<OpenApiIgnoreEnumAttribute>().Any())
                {
                    enumOpenApiStrings.Add(new OpenApiString(enumValue.ToString()));
                }
            }

            schema.Enum = enumOpenApiStrings;
        }
    }
}