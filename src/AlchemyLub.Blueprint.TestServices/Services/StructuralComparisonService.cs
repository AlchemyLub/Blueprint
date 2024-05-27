namespace AlchemyLub.Blueprint.TestServices.Services;

/// <summary>
/// Сервис для полных структурных сравнений
/// </summary>
public static class StructuralComparisonService
{
    /// <summary>
    /// Структурно сравнивает два любых типа
    /// </summary>
    /// <param name="firstType">Первый тип</param>
    /// <param name="secondType">Второй тип</param>
    /// <returns>Агрегированный результат проверки</returns>
    /// <exception cref="InvalidEnumArgumentException">Ошибка возникает, если невозможно определить упрощённый тип сравниваемых объектов</exception>
    public static AssertResult CompareTypes(Type firstType, Type secondType)
    {
        AssertResult result = new();

        if (firstType == secondType)
        {
            return result;
        }

        if (!firstType.TryGetTestType(secondType, out SimplifiedType? testType))
        {
            return result.AddError($"Типы [{firstType.FullName}] и [{secondType.FullName}] не соответствуют друг другу");
        }

        return testType switch
        {
            SimplifiedType.Primitive => result.AddError($"Типы {firstType.FullName} и {secondType.FullName}" +
                                                        $" являются базовыми и не соответствуют друг другу"),
            SimplifiedType.Enum => CompareEnums(firstType, secondType),
            SimplifiedType.CustomType => CompareCustomTypes(firstType, secondType),
            SimplifiedType.Unknown => result.AddError($"{firstType.FullName} является неизвестным типом"), // TODO: Нужна нормальная ошибка
            _ => throw new InvalidEnumArgumentException(nameof(testType), (int)testType, typeof(SimplifiedType))
        };
    }

    /// <summary>
    /// Структурно сравнивает два класса с контрактами
    /// </summary>
    /// <param name="firstContractType"></param>
    /// <param name="secondContractType"></param>
    /// <returns></returns>
    public static AssertResult CompareContracts(Type firstContractType, Type secondContractType)
    {
        AssertResult result = new();

        MethodMetadata[] controllerMethods = firstContractType.GetPublicInstanceMethods();
        MethodMetadata[] contractMethods = secondContractType.GetPublicInstanceMethods();

        if (controllerMethods.Length != contractMethods.Length)
        {
            result.AddError($"У классов [{firstContractType.FullName}] и [{secondContractType.FullName}] не совпадает количество методов");
        }

        if (controllerMethods.Length > 1)
        {
            Array.Sort(controllerMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(contractMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < controllerMethods.Length; i++)
        {
            result = result.Combine(CompareMethods(controllerMethods[i], contractMethods[i]));
        }

        return result;
    }

    /// <summary>
    /// Структурно сравнивает два метода
    /// </summary>
    /// <param name="firstMethodMetadata">Первый метод</param>
    /// <param name="secondMethodMetadata">Второй метод</param>
    /// <returns>Агрегированный результат проверки</returns>
    public static AssertResult CompareMethods(MethodMetadata firstMethodMetadata, MethodMetadata secondMethodMetadata)
    {
        AssertResult result = new();

        Type firstReturnType = firstMethodMetadata.ReturnType;
        Type secondReturnType = secondMethodMetadata.ReturnType;

        if (firstReturnType != secondReturnType)
        {
            result.Combine(CompareTypes(firstReturnType, secondReturnType));
        }

        MethodParameter[] firstParameters = firstMethodMetadata.Parameters;
        MethodParameter[] secondParameters = secondMethodMetadata.Parameters;

        if (firstParameters.Length != secondParameters.Length)
        {
            return result.AddError($"У методов [{firstMethodMetadata.Name}] и [{secondMethodMetadata.Name}] не совпадает количество параметров");
        }

        if (firstParameters.Length > 1)
        {
            Array.Sort(firstParameters, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(secondParameters, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < firstParameters.Length; i++)
        {
            MethodParameter firstParameterInfo = firstParameters[i];
            MethodParameter secondParameterInfo = secondParameters[i];

            if (firstParameterInfo.Name != secondParameterInfo.Name)
            {
                result.AddError($"Параметры [{firstParameterInfo.Name}] и [{secondParameterInfo.Name}] имеют разные имена");
            }

            result.Combine(CompareTypes(firstParameterInfo.Type, secondParameterInfo.Type));
        }

        return result;
    }


    /// <summary>
    /// Структурно сравнивает два метода
    /// </summary>
    /// <param name="firstMethodInfo">Первый метод</param>
    /// <param name="secondMethodInfo">Второй метод</param>
    /// <returns>Агрегированный результат проверки</returns>
    public static AssertResult CompareMethods(MethodInfo firstMethodInfo, MethodInfo secondMethodInfo)
    {
        MethodMetadata firstMethodMetadata = firstMethodInfo.ToMethodMetadata();
        MethodMetadata secondMethodMetadata = secondMethodInfo.ToMethodMetadata();

        return CompareMethods(firstMethodMetadata, secondMethodMetadata);
    }

    /// <summary>
    /// Структурно сравнивает два типа перечисления [<see langword="enum"/>]
    /// </summary>
    /// <param name="firstEnum">Первый тип перечисления [<see langword="enum"/>]</param>
    /// <param name="secondEnum">Второй тип перечисления [<see langword="enum"/>]</param>
    /// <returns>Агрегированный результат проверки</returns>
    public static AssertResult CompareEnums(Type firstEnum, Type secondEnum)
    {
        AssertResult result = new();

        if (!firstEnum.IsEnum)
        {
            return result.AddError($"{firstEnum.FullName} не является перечислением [{nameof(Enum)}]");
        }

        if (!secondEnum.IsEnum)
        {
            return result.AddError($"{secondEnum.FullName} не является перечислением [{nameof(Enum)}]");
        }

        EnumField[] firstFieldsInfo = firstEnum.GetEnumFields();
        EnumField[] secondFieldsInfo = secondEnum.GetEnumFields();

        if (firstFieldsInfo.Length != secondFieldsInfo.Length)
        {
            return result.AddError($"У перечислений [{firstEnum.FullName}] и [{secondEnum.FullName}] не совпадает количество значений");
        }

        if (firstFieldsInfo.Length > 1)
        {
            Array.Sort(firstFieldsInfo, (p1, p2) => p1.Value.CompareTo(p2.Value));
            Array.Sort(secondFieldsInfo, (p1, p2) => p1.Value.CompareTo(p2.Value));
        }

        for (int i = 0; i < firstFieldsInfo.Length; i++)
        {
            EnumField firstFieldInfo = firstFieldsInfo[i];
            EnumField secondFieldInfo = secondFieldsInfo[i];

            if (firstFieldInfo.Name != secondFieldInfo.Name || firstFieldInfo.Value != secondFieldInfo.Value)
            {
                result.AddError($"У перечислений [{firstEnum.FullName}] и [{secondEnum.FullName}] не совпадают значения " +
                                $"{firstFieldInfo.Value}. {firstFieldInfo.Name}" +
                                $" <-> {secondFieldInfo.Value}. {secondFieldInfo.Name}");
            }
        }

        return result;
    }

    /// <summary>
    /// Структурно сравнивает два пользовательских типа
    /// </summary>
    /// <param name="firstCustomType">Первый пользовательский тип</param>
    /// <param name="secondCustomType">Второй пользовательский тип</param>
    /// <returns>Агрегированный результат проверки</returns>
    public static AssertResult CompareCustomTypes(Type firstCustomType, Type secondCustomType)
    {
        AssertResult result = new();

        PropertyInfo[] firstTypeProperties = firstCustomType.GetProperties();
        PropertyInfo[] secondTypeProperties = secondCustomType.GetProperties();

        if (firstTypeProperties.Length != secondTypeProperties.Length)
        {
            return result.AddError($"У типов [{firstCustomType.FullName}] и [{secondCustomType.FullName}] не совпадает количество свойств");
        }

        if (firstTypeProperties.Length > 1)
        {
            Array.Sort(firstTypeProperties, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(secondTypeProperties, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < firstTypeProperties.Length; i++)
        {
            PropertyInfo firstPropertyInfo = firstTypeProperties[i];
            PropertyInfo secondPropertyInfo = secondTypeProperties[i];

            if (firstPropertyInfo.Name != secondPropertyInfo.Name)
            {
                return result.AddError(
                    $"У типов [{firstCustomType.FullName}] и [{secondCustomType.FullName}] свойства не совпадают по именам. " +
                    $"[{firstPropertyInfo.Name} != {secondPropertyInfo.Name}]");
            }

            Type firstPropertyType = firstTypeProperties[i].PropertyType;
            Type secondPropertyType = secondTypeProperties[i].PropertyType;

            AssertResult comparingTypesResult = CompareTypes(firstPropertyType, secondPropertyType);

            result.Combine(comparingTypesResult);
        }

        return result;
    }

    private static bool FilterCurrentInstanceMethods(MethodInfo methodInfo, Type type) =>
        methodInfo.DeclaringType == type && !methodInfo.CheckGeneratedAttributes();
}
