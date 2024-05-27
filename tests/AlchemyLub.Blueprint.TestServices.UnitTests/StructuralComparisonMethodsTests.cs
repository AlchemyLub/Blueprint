namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareMethods(MethodInfo, MethodInfo)"/>
/// </summary>
public class StructuralComparisonMethodsTests
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
    /// Проверяет структурное соответствие двух методов
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareMethods_Should_BeSuccessful(MethodInfo firstMethod, MethodInfo secondMethod, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareMethods(firstMethod, secondMethod);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
