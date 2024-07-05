namespace AlchemyLub.Blueprint.ArchTests;

public sealed class CodingRuleTests
{
    /// <summary>
    /// Тест проверяет, что все кастомные опции запечатаны
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
            .HaveNameEndingWith(Postfixes.Options)
            .Should()
            .BeSealed();

        TestResult result = conditionList.GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
