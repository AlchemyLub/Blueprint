using Microsoft.CodeAnalysis.CSharp;

namespace AlchemyLub.Blueprint.Generator;

[Generator]
public sealed class DecoratorGenerator : IIncrementalGenerator
{
    private const string DecoratorAttributeFileName = "DecoratorAttribute.g.cs";

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

        IncrementalValuesProvider<INamedTypeSymbol> pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            "DecoratorGenerator.Attributes.DecorateAttribute`1",
            (node, _) => node is ClassDeclarationSyntax classDeclaration
                         && classDeclaration.Members.OfType<MethodDeclarationSyntax>().Any()
                         && classDeclaration.BaseList is { Types.Count: > 0},
            (attributeContext, _) =>
            {
                if (attributeContext.TargetSymbol is not INamedTypeSymbol classSymbol)
                {
                    return null;
                }

                return classSymbol;
            })
            .Where(t => t is not null)!;

        context.RegisterSourceOutput(pipeline, Execute);
    }

    private void TestClass(INamedTypeSymbol classSymbol)
    {

    }

    private static void Execute(SourceProductionContext context, INamedTypeSymbol classSymbol)
    {
        if (classSymbol.IsAbstract)
        {
            return;
        }

        string code = GenerateCode();

        context.AddSource("DecoratorExtensions.g", code);
    }

    private static string GenerateCode() => string.Empty;
}