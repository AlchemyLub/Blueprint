namespace FloralHub.Blueprint.ArchTests;

/// <summary>
/// Тесты для проверки связей между слоями
/// </summary>
public class LayerTests : BaseTest
{
    /// <summary>
    /// Общий слой <see cref="SharedKernel"/> не должен содержать зависимости от других проектов
    /// </summary>
    [Fact]
    public void SharedKernelLayer_Should_BeIsolated()
    {
        TestResult? result = Types.InAssembly(SharedAssembly)
            .Should()
            .NotHaveDependencyOnAny(Application, App, Endpoints, Infrastructure, Domain)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    /// <summary>
    /// Доменный слой <see cref="Domain"/> не должен зависеть от слоёв верхних уровней
    /// </summary>
    [Fact]
    public void DomainLayer_Should_BeIsolated()
    {
        ConditionList? condition = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(Application, App, Endpoints, Infrastructure);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Доменный слой <see cref="Domain"/> может зависеть только от общего проекта
    /// </summary>
    [Fact]
    public void DomainLayer_Should_DependOnlySharedKernel()
    {
        ConditionList? condition = Types.InAssembly(DomainAssembly)
            .Should()
            .HaveDependencyOn(SharedKernel);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Слой бизнес логики <see cref="Application"/> не может зависеть от слоёв верхних уровней
    /// </summary>
    [Fact]
    public void ApplicationLayer_Should_BeIsolated()
    {
        TestResult? result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(App, Endpoints, Infrastructure)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    /// <summary>
    /// Слой бизнес логики <see cref="Application"/> может зависеть только от домена
    /// </summary>
    [Fact]
    public void ApplicationLayer_Should_DependOnlyDomain()
    {
        ConditionList? condition = Types.InAssembly(ApplicationAssembly)
            .Should()
            .HaveDependencyOn(Domain);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Инфраструктурный слой <see cref="Infrastructure"/> не может зависеть от слоёв того же уровня или уровня выше
    /// </summary>
    [Fact]
    public void InfrastructureLayer_Should_BeIsolated()
    {
        TestResult? testResult = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOnAny(App, Endpoints, SharedKernel)
            .GetResult();

        bool result = testResult.IsSuccessful;

        result.Should().BeTrue();
    }

    /// <summary>
    /// Инфраструктурный слой <see cref="Infrastructure"/> может зависеть только от слоя бизнес логики
    /// </summary>
    [Fact]
    public void InfrastructureLayer_Should_DependOnlyApplication()
    {
        ConditionList? condition = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .HaveDependencyOn(Application);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Презентационный слой <see cref="Endpoints"/> не может зависеть от слоёв того же уровня или уровня выше
    /// </summary>
    [Fact]
    public void EndpointsLayer_Should_BeIsolated()
    {
        TestResult? testResult = Types.InAssembly(EndpointsAssembly)
            .Should()
            .NotHaveDependencyOnAny(App, Infrastructure, SharedKernel)
            .GetResult();

        bool result = testResult.IsSuccessful;

        result.Should().BeTrue();
    }

    /// <summary>
    /// Презентационный слой <see cref="Endpoints"/> может зависеть только от слоя бизнес логики
    /// </summary>
    [Fact]
    public void EndpointsLayer_Should_DependOnlyApplication()
    {
        ConditionList? condition = Types.InAssembly(EndpointsAssembly)
            .Should()
            .HaveDependencyOn(Application);

        int result = condition.Count();

        result.Should().BePositive();
    }
}
