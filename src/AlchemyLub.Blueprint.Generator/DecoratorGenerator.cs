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

        IncrementalValueProvider<ImmutableArray<ClassModel>> pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
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

    private static ClassModel CreateClassModel(
        ClassDeclarationSyntax classDeclaration,
        INamedTypeSymbol classSymbol,
        IEnumerable<string> decoratorNames)
    {
        IReadOnlyCollection<MethodModel> methods =
            ClassGenerator.GenerateMethods(classDeclaration.Members.OfType<MethodDeclarationSyntax>());

        ClassBuilder classBuilder = new(classSymbol.Name);

        classBuilder.WithMethods(methods.ToArray());

        return classBuilder.Build();
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<ClassModel> classModels)
    {
        string code = GenerateCode(classModels);

        context.AddSource("DecoratorExtensions.g.cs", code);
    }

    private static string GenerateCode(ImmutableArray<ClassModel> classModels) =>
$"""
{ClassComponents.ExtensionClassHead}
        {string.Join(SyntaxFactory.CarriageReturnLineFeed.ToString(), classModels.Select(GenerateDecorateRows))}

{ClassComponents.ExtensionClassBasement}
""";

    private static string GenerateDecorateRows(ClassModel classModel) =>
        string.Join(
            SyntaxFactory.CarriageReturnLineFeed.ToString(),
            classModel.DecoratorNames.Select(t => $"services.Decorate<{classModel.InterfaceName},{t}>;"));
}
