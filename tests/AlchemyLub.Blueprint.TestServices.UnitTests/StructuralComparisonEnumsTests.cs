namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareEnums"/>
/// </summary>
public sealed class StructuralComparisonEnumsTests
{
    public static TheoryData<Type, Type, bool> Data =>
        new()
        {
            { typeof(UserType), typeof(SameUserType), true },
            { typeof(UserRoles), typeof(SameUserRoles), true },
            { typeof(UserRoles), typeof(SameUserType), false },
            { typeof(UserType), typeof(UserRoles), false },
            { typeof(UserType), typeof(WrongUserType), false },
            { typeof(UserRoles), typeof(WrongUserRoles), false },
            { typeof(TestUser), typeof(SameUserType), false },
            { typeof(UserType), typeof(SameTestUser), false },
            { typeof(SameTestUser), typeof(SameTestUser), false }
        };

    /// <summary>
    /// Проверяет структурное соответствие двух перечислений [<see langword="enum"/>]
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareEnums_Should_BeSuccessful(Type firstType, Type secondType, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareEnums(firstType, secondType);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
