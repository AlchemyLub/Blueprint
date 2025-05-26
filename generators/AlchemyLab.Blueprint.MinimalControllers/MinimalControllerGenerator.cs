namespace AlchemyLab.Blueprint.MinimalControllers.Generator;

/// <summary>
/// Генератор кода для регистрации контроллеров в MinimalAPI
/// </summary>
[Generator]
public sealed class MinimalControllerGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif

        // Новый API для поиска по атрибутам
        IncrementalValuesProvider<ControllerInfo?> controllerInfos = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AttributeNames.MinimalControllerAttributeName,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (attributeSyntaxContext, _) => ControllersAnalyzer.GetControllerInfos(attributeSyntaxContext));

        // Регистрируем файл расширения для всех контроллеров
        context.RegisterSourceOutput(
            controllerInfos.Where(t => t.HasValue).Select((x, _) => x!.Value).Collect(), // TODO: Корявое решение, исправить
            static (sourceProductionContext, controllers) =>
                RawMinimalControllerFactory.GenerateExtensionFile(sourceProductionContext, controllers));
    }
}
