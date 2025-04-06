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
        // Получаем классы с атрибутом ApiController
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (ctx, _) => GetClassIfHasAttribute(ctx))
            .Where(static c => c is not null)!;

        // Собираем информацию о контроллерах
        IncrementalValuesProvider<ControllerInfo> controllerInfos = context.CompilationProvider
            .Combine(classDeclarations.Collect())
            .SelectMany((tuple, _) =>
                GetControllerInfos(tuple.Left, tuple.Right));

        // Генерируем код для каждого контроллера
        IncrementalValueProvider<(Compilation, ImmutableArray<ControllerInfo>)> compilationAndControllers =
            context.CompilationProvider
                .Combine(controllerInfos.Collect());

        // Регистрируем файл расширения для всех контроллеров
        context.RegisterSourceOutput(
            compilationAndControllers,
            static (sourceProductionContext, tuple) =>
                GenerateExtensionFiles(sourceProductionContext, tuple.Item2));
    }

    private static ClassDeclarationSyntax? GetClassIfHasAttribute(GeneratorSyntaxContext ctx)
    {
        ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)ctx.Node;

        SemanticModel model = ctx.SemanticModel;

        foreach (AttributeListSyntax attributeList in classDeclaration.AttributeLists)
        {
            foreach (AttributeSyntax attribute in attributeList.Attributes)
            {
                ISymbol? attributeSymbol = model.GetSymbolInfo(attribute).Symbol;

                if (attributeSymbol?.ContainingType?.ToDisplayString() is MinimalControllerAttributeNames.ApiControllerAttributeName)
                {
                    return classDeclaration;
                }
            }
        }

        return null;
    }

    private static IEnumerable<ControllerInfo> GetControllerInfos(
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classes)
    {
        INamedTypeSymbol? apiControllerAttributeSymbol = compilation.GetTypeByMetadataName(MinimalControllerAttributeNames.ApiControllerAttributeName);
        INamedTypeSymbol? httpMethodAttributeSymbol = compilation.GetTypeByMetadataName(MinimalControllerAttributeNames.HttpMethodAttributeBaseName);
        INamedTypeSymbol? authAttributeSymbol = compilation.GetTypeByMetadataName(MinimalControllerAttributeNames.AuthAttributeBaseName);

        if (apiControllerAttributeSymbol is null || httpMethodAttributeSymbol is null)
        {
            yield break;
        }

        foreach (ClassDeclarationSyntax classDeclaration in classes)
        {
            SemanticModel model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            if (model.GetDeclaredSymbol(classDeclaration) is not { } classSymbol)
            {
                continue;
            }

            // Проверяем наличие ApiController атрибута
            AttributeData? apiControllerAttribute = classSymbol.GetAttributes()
                .FirstOrDefault(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, apiControllerAttributeSymbol));

            if (apiControllerAttribute?.ConstructorArguments.Length is not 1)
            {
                continue;
            }

            // Получаем базовый путь контроллера
            string? route = apiControllerAttribute.ConstructorArguments[0].Value?.ToString();

            if (string.IsNullOrEmpty(route))
            {
                continue;
            }

            // Проверяем инжекцию зависимостей и собираем типы зависимостей
            List<string> dependencyTypes = [];

            foreach (var constructor in classSymbol.Constructors.Where(c => c.DeclaredAccessibility == Accessibility.Public))
            {
                foreach (var parameter in constructor.Parameters)
                {
                    dependencyTypes.Add(parameter.Type.ToDisplayString());
                }
            }

            // Получаем все методы контроллера с HTTP-атрибутами
            List<EndpointInfo> endpoints = [];

            foreach (ISymbol member in classSymbol.GetMembers())
            {
                if (member is not IMethodSymbol { MethodKind: MethodKind.Ordinary } methodSymbol)
                {
                    continue;
                }

                // Проверяем, является ли метод публичным
                if (methodSymbol.DeclaredAccessibility is not Accessibility.Public)
                {
                    continue;
                }

                // Проверяем наличие HTTP-атрибута
                AttributeData? httpAttribute = methodSymbol
                    .GetAttributes()
                    .FirstOrDefault(attr =>
                        attr.AttributeClass?.BaseType is not null
                        && SymbolEqualityComparer.Default.Equals(attr.AttributeClass.BaseType, httpMethodAttributeSymbol));

                if (httpAttribute is null)
                {
                    // Выдаем диагностику: метод должен иметь HTTP-атрибут
                    Diagnostic diagnostic = Diagnostic.Create(
                        DiagnosticDescriptors.MissingHttpMethodAttribute,
                        methodSymbol.Locations[0],
                        methodSymbol.Name,
                        classSymbol.Name,
                        methodSymbol.DeclaredAccessibility.ToString());

                    continue;
                }

                // Получаем HTTP-метод
                string? httpMethod = httpAttribute.AttributeClass.GetHttpMethod();

                if (httpMethod is null)
                {
                    continue;
                }

                // Получаем путь эндпоинта
                string endpointRoute = "";

                if (httpAttribute.ConstructorArguments.Length > 0)
                {
                    endpointRoute = httpAttribute.ConstructorArguments[0].Value?.ToString() ?? string.Empty;
                }

                // Формируем полный путь
                string fullRoute = CombineRoutes(route, endpointRoute);

                // Получаем параметры метода
                ImmutableArray<ParameterInfo> parameters = [
                    ..methodSymbol.Parameters
                        .Select(p => new ParameterInfo(
                            p.Name,
                            p.Type.ToDisplayString(),
                            // Простое правило: параметры из тела имеют атрибут [FromBody] или сложные типы, не примитивы
                            !p.Type.IsPrimitive()))
                ];

                // Проверяем, есть ли параметры маршрута в URL
                bool hasRouteParameters = fullRoute.Contains("{") && fullRoute.Contains("}");

                // Проверяем, есть ли параметры из тела
                bool hasBodyParameter = parameters.Any(p => p.IsBody);

                // Проверяем, является ли метод асинхронным
                bool isAsync = methodSymbol.IsAsync
                               || methodSymbol.ReturnType is INamedTypeSymbol { Name: "Task" or "ValueTask" };

                // Проверяем наличие атрибута авторизации
                bool requiresAuth = authAttributeSymbol is not null
                                    && (classSymbol
                                            .GetAttributes()
                                            .Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, authAttributeSymbol))
                                        || methodSymbol
                                            .GetAttributes()
                                            .Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, authAttributeSymbol)));

                endpoints.Add(new EndpointInfo(
                    methodSymbol.Name,
                    httpMethod,
                    fullRoute,
                    methodSymbol.ReturnType.ToDisplayString(),
                    parameters,
                    hasBodyParameter,
                    hasRouteParameters,
                    isAsync,
                    requiresAuth));
            }

            if (endpoints.Count > 0)
            {
                yield return new ControllerInfo(
                    classSymbol.Name,
                    classSymbol.ContainingNamespace.ToDisplayString(),
                    route,
                    [..endpoints],
                    [..dependencyTypes]);
            }
        }
    }

    private static string CombineRoutes(string baseRoute, string endpointRoute)
    {
        if (string.IsNullOrEmpty(endpointRoute))
        {
            return baseRoute;
        }

        // Убираем лишние слэши на стыке путей
        if (baseRoute.EndsWith("/"))
        {
            baseRoute = baseRoute.TrimEnd('/');
        }

        if (endpointRoute.StartsWith("/"))
        {
            endpointRoute = endpointRoute.TrimStart('/');
        }

        return $"{baseRoute}/{endpointRoute}";
    }

    private static void GenerateExtensionFiles(
        SourceProductionContext context,
        ImmutableArray<ControllerInfo> controllers)
    {
        if (controllers.IsDefaultOrEmpty)
        {
            return;
        }

        // Генерируем файл расширения для WebApplication
        var source = GenerateExtensionFile(controllers);
        context.AddSource("MinimalControllers.Extensions.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateExtensionFile(ImmutableArray<ControllerInfo> controllers)
    {
        var sb = new StringBuilder();

        sb.AppendLine("""
                      // <auto-generated />
                      #nullable enable

                      namespace Microsoft.AspNetCore.Builder;

                      /// <summary>
                      /// Методы расширения для регистрации контроллеров MinimalApi
                      /// </summary>
                      public static class MinimalControllersExtensions
                      {
                          /// <summary>
                          /// Регистрирует все контроллеры MinimalApi
                          /// </summary>
                          /// <param name="app">Экземпляр приложения</param>
                          /// <returns>Экземпляр приложения для цепочки вызовов</returns>
                          public static WebApplication MapMinimalControllers(this WebApplication app)
                          {
                      """);

        foreach (ControllerInfo controller in controllers)
        {
            sb.AppendLine($"        app.MapController{controller.Name}();");
        }

        sb.AppendLine("""
                              return app;
                          }
                      """);

        // Генерируем методы для каждого контроллера
        foreach (ControllerInfo controller in controllers)
        {
            sb.AppendLine();
            sb.AppendLine($$"""
                                /// <summary>
                                /// Регистрирует контроллер {{controller.Name}}
                                /// </summary>
                                /// <param name="app">Экземпляр приложения</param>
                                /// <returns>Экземпляр приложения для цепочки вызовов</returns>
                                public static WebApplication MapController{{controller.Name}}(this WebApplication app)
                                {
                            """);

            foreach (EndpointInfo endpoint in controller.Endpoints)
            {
                sb.AppendLine(
                    $"        app.{endpoint.HttpMethod}(\"{endpoint.Route}\", {GenerateHandlerExpression(controller, endpoint)});");
            }

            sb.AppendLine("        return app;");
            sb.AppendLine("    }");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateHandlerExpression(ControllerInfo controller, EndpointInfo endpoint)
    {
        string parameters = string.Join(", ", endpoint.Parameters.Select(p => p.Name));
        string methodCall = $"{(endpoint.HasBodyParameter ? "await " : "")}controller.{endpoint.MethodName}({parameters})";

        return $"({string.Join(", ", endpoint.Parameters.Select(p => p.Type + " " + p.Name))}) => {{\n" +
               $"            var controller = app.Services.GetRequiredService<{controller.Namespace}.{controller.Name}>();\n" +
               $"            return {methodCall};\n" +
               $"        }}";
    }
}
