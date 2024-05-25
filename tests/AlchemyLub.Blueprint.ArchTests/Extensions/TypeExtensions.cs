namespace AlchemyLub.Blueprint.ArchTests.Extensions;

/// <summary>
/// Методы расширений для <see cref="Type"/>
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Типы, содержащиеся в BCL. Они тривиально сравниваются друг с другом.
    /// </summary>
    private static readonly FrozenSet<Type> BclTypes = new[]
    {
        typeof(string),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateOnly),
        typeof(TimeOnly),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid)
    }.ToFrozenSet();

    /// <summary>
    /// Проверяет простой ли это тип
    /// </summary>
    /// <param name="type">Проверяемый тип</param>
    /// <returns>
    /// Возвращает <see langword="true"/> если текущий тип является простым и не требует сложного сравнения, иначе возвращает <see langword="false"/>
    /// </returns>
    internal static bool IsSimpleType(this Type type)
    {
        if (type.IsPrimitive || BclTypes.Contains(type))
        {
            return true;
        }

        return TryUnboxNullableType(type, out Type? unboxType) && IsSimpleType(unboxType);
    }

    /// <summary>
    /// Возвращает <see cref="SimplifiedType"/> соответствующий обоим типам
    /// </summary>
    /// <param name="firstType">Первый тип для сравнения</param>
    /// <param name="secondType">Второй тип для сравнения</param>
    /// <param name="testType">Тестовый тип, представляющий упрощённую типизацию</param>
    /// <returns>
    /// Возвращает <see langword="true"/> если вычисленный <see cref="SimplifiedType"/> у двух типов одинаковый, иначе возвращает <see langword="false"/>
    /// </returns>
    internal static bool TryGetTestType(this Type firstType, Type secondType, [NotNullWhen(true)] out SimplifiedType? testType)
    {
        SimplifiedType firstTestType = GetTestType(firstType);
        SimplifiedType secondTestType = GetTestType(secondType);

        if (firstTestType == secondTestType)
        {
            testType = firstTestType;
            return true;
        }

        testType = null;
        return false;
    }

    private static bool TryUnboxNullableType(Type type, [NotNullWhen(true)] out Type? unboxNullableType)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(type);

        if (underlyingType is null)
        {
            unboxNullableType = underlyingType;
            return false;
        }

        unboxNullableType = underlyingType;
        return true;
    }

    private static SimplifiedType GetTestType(Type type)
    {
        if (type.IsEnum)
        {
            return SimplifiedType.Enum;
        }

        return type.IsSimpleType()
            ? SimplifiedType.Primitive
            : SimplifiedType.CustomType;
    }
}
