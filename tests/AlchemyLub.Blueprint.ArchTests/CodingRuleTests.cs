namespace AlchemyLub.Blueprint.ArchTests;

public sealed class CodingRuleTests
{
    /// <summary>
    /// Тест должен проверять, что все кастомные опции запечатаны. Сейчас работает некорректно!
    /// </summary>
    [Fact]
    public void Options_Should_BeSealed()
    {
        ConditionList conditionList = Types.InAssemblies(new[]
            {
                Assemblies.ApplicationAssembly,
                Assemblies.ClientsAssembly,
                Assemblies.DomainAssembly,
                Assemblies.EndpointsAssembly,
                Assemblies.InfrastructureAssembly
            })
            .That()
            .HaveNameEndingWith("Options")
            .Should()
            .BeSealed();

        int result = conditionList.Count();

        result.Should().Be(int.MaxValue);
    }
}
