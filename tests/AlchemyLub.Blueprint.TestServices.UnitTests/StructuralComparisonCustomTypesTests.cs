namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareCustomTypes"/>
/// </summary>
public class StructuralComparisonCustomTypesTests
{
    public static TheoryData<Type, Type, bool> Data =>
        new()
        {
            { typeof(TestUser), typeof(SameTestUser), true },
            { typeof(City), typeof(SameCity), true },
            { typeof(Work), typeof(SameWork), true },
            { typeof(TestUser), typeof(WrongTestUser), false },
            { typeof(City), typeof(WrongCity), false },
            { typeof(Work), typeof(WrongWork), false },
            { typeof(Work), typeof(SameUserType), false },
            { typeof(UserType), typeof(WrongWork), false }
        };

    /// <summary>
    /// Проверяет структурное соответствие двух пользовательских типов
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareCustomTypes_Should_BeSuccessful(Type firstType, Type secondType, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareCustomTypes(firstType, secondType);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
