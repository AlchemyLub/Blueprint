namespace AlchemyLub.Blueprint.TestServices.UnitTests;

/// <summary>
/// Тесты для <see cref="StructuralComparisonService.CompareMethods(MethodInfo, MethodInfo)"/>
/// </summary>
public class StructuralComparisonMethodsTests
{
    private static readonly MethodMetadata[] TestServiceMethods = typeof(TestService).GetPublicInstanceMethods();
    private static readonly MethodMetadata[] SameTestServiceMethods = typeof(SameTestService).GetPublicInstanceMethods();
    private static readonly MethodMetadata[] WrongTestServiceMethods = typeof(WrongTestService).GetPublicInstanceMethods();

    public static TheoryData<MethodMetadata, MethodMetadata, bool> Data =>
        new()
        {
            { TestServiceMethods[0], SameTestServiceMethods[0], true },
            { TestServiceMethods[1], SameTestServiceMethods[1], true },
            { TestServiceMethods[2], SameTestServiceMethods[2], true },
            { TestServiceMethods[0], WrongTestServiceMethods[0], false },
            { TestServiceMethods[1], WrongTestServiceMethods[1], false },
            { TestServiceMethods[2], WrongTestServiceMethods[2], false }
        };

    /// <summary>
    /// Статический конструктор для предварительной сортировки методов по имени.
    /// </summary>
    static StructuralComparisonMethodsTests()
    {
        Array.Sort(TestServiceMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        Array.Sort(SameTestServiceMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        Array.Sort(WrongTestServiceMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
    }

    /// <summary>
    /// Проверяет структурное соответствие двух методов
    /// </summary>
    [Theory]
    [MemberData(nameof(Data))]
    public void CompareMethods_Should_BeSuccessful(MethodMetadata firstMethod, MethodMetadata secondMethod, bool result)
    {
        AssertResult assertResult = StructuralComparisonService.CompareMethods(firstMethod, secondMethod);

        Assert.Equal(assertResult.IsSuccessful, result);
    }
}
