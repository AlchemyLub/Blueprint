namespace AlchemyLab.Blueprint.MinimalControllers.Generator;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MinimalControllersCodeFixProvider)), Shared]
public sealed class MinimalControllersCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [DiagnosticDescriptors.NonPublicHttpMethod.Id];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        Diagnostic? diagnostic = context.Diagnostics.FirstOrDefault(d => d.Id == DiagnosticDescriptors.NonPublicHttpMethod.Id);

        if (diagnostic is null)
        {
            return;
        }

        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

        if (root?.FindNode(diagnosticSpan) is not MethodDeclarationSyntax methodDeclaration)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Make method public",
                createChangedSolution: c => MakeMethodPublic(context.Document, methodDeclaration, c),
                equivalenceKey: "MakeMethodPublic"),
            diagnostic);
    }

    private static async Task<Solution> MakeMethodPublic(
        Document document,
        MethodDeclarationSyntax methodDeclaration,
        CancellationToken cancellationToken)
    {
        DocumentEditor? editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        // Удалите существующий модификатор (если таковой имеется) и добавьте public
        SyntaxTokenList newModifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddRange(methodDeclaration.Modifiers.Where(m => m.IsKind(SyntaxKind.AsyncKeyword))); // Оставьте async, если присутствует

        MethodDeclarationSyntax newMethod = methodDeclaration.WithModifiers(newModifiers);

        editor.ReplaceNode(methodDeclaration, newMethod);

        return editor.GetChangedDocument().Project.Solution;
    }
}
