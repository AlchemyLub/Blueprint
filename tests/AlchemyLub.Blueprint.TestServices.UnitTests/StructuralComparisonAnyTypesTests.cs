namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareTypes"/>
/// </summary>
public class StructuralComparisonAnyTypesTests
{
    public static TheoryData<Type, Type, bool> Data =>
        new()
        {
            { typeof(Guid), typeof(Guid), true },
            { typeof(Guid), typeof(string), false },
            { typeof(TestUser), typeof(SameTestUser), true },
            { typeof(City), typeof(SameCity), true },
            { typeof(Work), typeof(SameWork), true },
            { typeof(TestUser), typeof(WrongTestUser), false },
            { typeof(City), typeof(WrongCity), false },
            { typeof(Work), typeof(WrongWork), false },
            { typeof(Work), typeof(SameUserType), false },
            { typeof(UserType), typeof(WrongWork), false },
            { typeof(UserType), typeof(SameUserType), true },
            { typeof(UserRoles), typeof(SameUserRoles), true },
            { typeof(UserRoles), typeof(UserRoles), true },
            { typeof(UserType), typeof(UserType), true },
            { typeof(UserRoles), typeof(SameUserType), false },
            { typeof(UserType), typeof(UserRoles), false },
            { typeof(UserType), typeof(WrongUserType), false },
            { typeof(UserRoles), typeof(WrongUserRoles), false },
            { typeof(WrongUserRoles), typeof(WrongUserType), false },
            { typeof(TestUser), typeof(SameUserType), false },
            { typeof(UserType), typeof(SameTestUser), false },
            { typeof(SameTestUser), typeof(SameTestUser), true }
        };

    /// <summary>
    /// Проверяет структурное соответствие двух любых типов
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareTypes_Should_BeSuccessful(Type firstType, Type secondType, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareTypes(firstType, secondType);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
