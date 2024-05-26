namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService"/>
/// </summary>
public class StructuralComparisonServiceTests
{
    [Fact]
    public void CompareEnumTypes_SimilarEnums_SuccessfullyResult()
    {
        Type userType = typeof(UserType);
        Type sameUserType = typeof(SameUserType);

        AssertResult assertResult = StructuralComparisonService.CompareEnumTypes(userType, sameUserType);

        Assert.True(assertResult.IsSuccessful);
    }

    [Fact]
    public void CompareEnumTypes_DifferentEnums_WrongResult()
    {
        Type userType = typeof(UserType);
        Type wrongUserType = typeof(WrongUserType);

        AssertResult assertResult = StructuralComparisonService.CompareEnumTypes(userType, wrongUserType);

        Assert.False(assertResult.IsSuccessful);
    }

    [Fact]
    public void CompareEnumFlags_SimilarEnums_SuccessfullyResult()
    {
        Type userRoles = typeof(UserRoles);
        Type sameUserRoles = typeof(SameUserRoles);

        AssertResult assertResult = StructuralComparisonService.CompareEnumTypes(userRoles, sameUserRoles);

        Assert.True(assertResult.IsSuccessful);
    }

    [Fact]
    public void CompareEnumFlags_DifferentEnums_WrongResult()
    {
        Type userRoles = typeof(UserRoles);
        Type wrongUserRoles = typeof(WrongUserRoles);

        AssertResult assertResult = StructuralComparisonService.CompareEnumTypes(userRoles, wrongUserRoles);

        Assert.False(assertResult.IsSuccessful);
    }
}
