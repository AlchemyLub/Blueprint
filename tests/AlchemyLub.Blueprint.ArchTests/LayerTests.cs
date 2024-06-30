namespace AlchemyLub.Blueprint.ArchTests;

/// <summary>
/// Тесты для проверки связей между слоями
/// </summary>
public sealed class LayerTests
{
    /// <summary>
    /// Слой клиентов <see cref="Clients"/> не должен зависеть от других слоёв
    /// </summary>
    [Fact]
    public void ClientsLayer_Should_BeIsolated()
    {
        ConditionList condition = Types.InAssembly(Assemblies.ClientsAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                FullProjectNames.App,
                FullProjectNames.Application,
                FullProjectNames.Domain,
                FullProjectNames.Endpoints,
                FullProjectNames.Infrastructure);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Доменный слой <see cref="Domain"/> не должен зависеть от слоёв верхних уровней
    /// </summary>
    [Fact]
    public void DomainLayer_Should_BeIsolated()
    {
        ConditionList condition = Types.InAssembly(Assemblies.DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                FullProjectNames.Application,
                FullProjectNames.App,
                FullProjectNames.Endpoints,
                FullProjectNames.Infrastructure,
                FullProjectNames.Clients);

        int result = condition.Count();

        result.Should().BePositive();
    }

    /// <summary>
    /// Слой бизнес логики <see cref="Application"/> не может зависеть от слоёв верхних уровней
    /// </summary>
    [Fact]
    public void ApplicationLayer_Should_BeIsolated()
    {
        ConditionList condition = Types.InAssembly(Assemblies.ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                FullProjectNames.App,
                FullProjectNames.Endpoints,
                FullProjectNames.Infrastructure,
                FullProjectNames.Clients);

        bool result = condition.GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    /// <summary>
    /// Инфраструктурный слой <see cref="Infrastructure"/> не может зависеть от слоёв того же уровня или уровня выше
    /// </summary>
    [Fact]
    public void InfrastructureLayer_Should_BeIsolated()
    {
        ConditionList condition = Types.InAssembly(Assemblies.InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOnAny(FullProjectNames.App, FullProjectNames.Endpoints, FullProjectNames.Clients);

        bool result = condition.GetResult().IsSuccessful;

        result.Should().BeTrue();
    }

    /// <summary>
    /// Презентационный слой <see cref="Endpoints"/> не может зависеть от слоёв того же уровня или уровня выше
    /// </summary>
    [Fact]
    public void EndpointsLayer_Should_BeIsolated()
    {
        ConditionList condition = Types.InAssembly(Assemblies.EndpointsAssembly)
            .Should()
            .NotHaveDependencyOnAny(FullProjectNames.App, FullProjectNames.Infrastructure, FullProjectNames.Clients);

        bool result = condition.GetResult().IsSuccessful;

        result.Should().BeTrue();
    }
}
