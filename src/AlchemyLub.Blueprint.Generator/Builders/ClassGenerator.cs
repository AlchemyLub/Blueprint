namespace AlchemyLub.Blueprint.Generator.Builders;

public static class ClassGenerator
{
    //public static string Generate(ClassModel classModel)
    //{
    //    return string.Empty;
    //}

    //public static FieldModel GenerateField(IReadOnlyCollection<FieldModel> fields)
    //{

    //}

    //public static IReadOnlyCollection<FieldModel> GenerateFields(IReadOnlyCollection<FieldModel> fields)
    //{

    //}

    //public static string GenerateConstant(IReadOnlyCollection<ConstantModel> constants)
    //{

    //}

    //public static string GenerateConstructor(ConstructorModel constructor)
    //{

    //}

    public static MethodModel GenerateMethod(MethodDeclarationSyntax methodSyntax)
    {
        IReadOnlyCollection<ParameterModel> parameters = GenerateMethodParameters(methodSyntax.ParameterList.Parameters);

        return new(methodSyntax.Identifier.Text, methodSyntax.ReturnType.ToFullString(), parameters);
    }

    public static IReadOnlyCollection<MethodModel> GenerateMethods(IEnumerable<MethodDeclarationSyntax> methodSyntax) =>
        methodSyntax.Select(GenerateMethod).ToArray();

    private static ParameterModel GenerateMethodParameter(ParameterSyntax parameterSyntax)
    {
        if (parameterSyntax.Type is null)
        {
            throw new ArgumentNullException(nameof(parameterSyntax.Type));
        }

        return new(
            parameterSyntax.Type.ToFullString(),
            parameterSyntax.Identifier.Text,
            parameterSyntax.Modifiers.Any()
                ? parameterSyntax.Modifiers.ToFullString()
                : string.Empty,
            parameterSyntax.Default?.Value.ToFullString());
    }

    private static IReadOnlyCollection<ParameterModel> GenerateMethodParameters(
        SeparatedSyntaxList<ParameterSyntax> parameterSyntaxList) =>
        parameterSyntaxList.Select(GenerateMethodParameter).ToArray();
}
