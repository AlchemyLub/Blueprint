namespace FloralHub.Blueprint.ArchTests.Common;

/// <summary>
/// Тесты для проверки связей между слоями
/// </summary>
public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_BeIsolated()
    {
        TestResult? result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAll(Application, App, Endpoints, Infrastructure)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_BeIsolated()
    {
        TestResult? result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAll(App, Endpoints, Infrastructure)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    // TODO: Описать остальные слои
}
