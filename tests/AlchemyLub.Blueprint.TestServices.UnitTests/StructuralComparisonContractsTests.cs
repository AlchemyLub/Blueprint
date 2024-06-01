namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareContracts"/>
/// </summary>
public class StructuralComparisonContractsTests
{
    public static TheoryData<Type, Type, bool> Data =>
        new()
        {
            { typeof(TestService), typeof(SameTestService), true },
            { typeof(TestService), typeof(WrongTestService), false }
        };

    /// <summary>
    /// Проверяет структурное соответствие двух классов-контрактов
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareContracts_Should_BeSuccessful(Type firstContractClass, Type secondContractClass, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareContracts(firstContractClass, secondContractClass);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
