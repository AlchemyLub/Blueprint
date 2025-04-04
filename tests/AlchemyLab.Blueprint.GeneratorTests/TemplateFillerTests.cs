namespace AlchemyLab.Blueprint.GeneratorTests;

public class TemplateFillerTests
{
    [Fact]
    public void GenerateConstructor_SuccessResult()
    {
        const string expectedCtor =
"""
    public ServiceTraceDecorator(
        IService wrappedService,
        IRepository repository,
        IServiceProvider provider,
        IExternalService externalService)
    {
        this.wrappedService = wrappedService;
        this.repository = repository;
        this.provider = provider;
        this.externalService = externalService;
    }
""";

        string result = TemplateFiller.GenerateConstructor(
            "IService",
            "Service",
            "TraceDecorator",
            new List<ParameterModel>
            {
                new("IRepository", "repository"),
                new("IServiceProvider", "provider"),
                new("IExternalService", "externalService")
            });

        Assert.Equal(expectedCtor, result);
    }
}
