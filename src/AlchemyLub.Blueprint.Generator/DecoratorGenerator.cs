namespace AlchemyLub.Blueprint.Generator;

[Generator]
public sealed class DecoratorGenerator : IIncrementalGenerator
{
    private const string DecoratorAttributeFileName = "DecoratorAttribute.g.cs";
    private const string DependencyInjectionDecoratorFileName = "DependencyInjectionDecorator.g.cs";

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }

        context.RegisterPostInitializationOutput(postInitContext => postInitContext.AddSource(
            DecoratorAttributeFileName,
            SourceText.From(RawAttributes.DecoratorAttributes, Encoding.UTF8)));

        context.RegisterPostInitializationOutput(postInitContext => postInitContext.AddSource(
            DependencyInjectionDecoratorFileName,
            SourceText.From(RawDecorators.DependencyInjectionDecoratorMethods, Encoding.UTF8)));

        IncrementalValueProvider<ImmutableArray<DecoratedClassModel>> pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            "DecoratorGenerator.Attributes.DecorateAttribute`1",
            (node, _) => node is ClassDeclarationSyntax classDeclaration
                         && classDeclaration.Members.OfType<MethodDeclarationSyntax>().Any()
                         && classDeclaration.BaseList is { Types.Count: > 0 },
            (attributeContext, _) =>
            {
                if (attributeContext.TargetSymbol is not INamedTypeSymbol classSymbol)
                {
                    return default;
                }

                if (attributeContext.TargetNode is not ClassDeclarationSyntax classDeclaration)
                {
                    return default;
                }

                IEnumerable<string> decoratorNames = attributeContext.Attributes
                    .Where(t => t.AttributeClass!.Name
                        is "DecorateAttribute"
                        or "DecorateAttribute`1"
                        or "Decorate")
                    .Select(t => t.ConstructorArguments.First().Value!.ToString());

                return CreateClassModel(classDeclaration, classSymbol, decoratorNames);
            })
            .Where(t => t != default)
            .Collect();

        context.RegisterSourceOutput(pipeline, Execute);
    }

    private static DecoratedClassModel CreateClassModel(
        ClassDeclarationSyntax classDeclaration,
        INamedTypeSymbol classSymbol,
        IEnumerable<string> decoratorNames)
    {
        string className = classSymbol.Name;

        ReadOnlyCollection<MethodModel> methods = classDeclaration.Members
            .OfType<MethodDeclarationSyntax>()
            .Select(m =>
                new MethodModel(
                    m.Identifier.Text,
                    m.ParameterList.Parameters.Select(t =>
                        new ParameterModel(
                            t.Type!.ToFullString(),
                            t.ToString()))
                        .ToList()
                        .AsReadOnly(),
                    m.ReturnType.ToFullString()))
            .ToList()
            .AsReadOnly();

        return new(
            className,
            methods,
            decoratorNames.ToList().AsReadOnly());
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<DecoratedClassModel> classModels)
    {
        string code = GenerateCode(classModels);

        context.AddSource("DecoratorExtensions.g.cs", code);
    }

    private static string GenerateCode(ImmutableArray<DecoratedClassModel> classModels) =>
$"""
{ClassComponents.ExtensionClassHead}
        {string.Join(SyntaxFactory.CarriageReturnLineFeed.ToString(), classModels.Select(GenerateDecorateRows))}

{ClassComponents.ExtensionClassBasement}
""";

    private static string GenerateDecorateRows(DecoratedClassModel classModel) =>
        string.Join(
            SyntaxFactory.CarriageReturnLineFeed.ToString(),
            classModel.DecoratorNames.Select(t => $"services.Decorate<{classModel.InterfaceName},{t}>;"));
}