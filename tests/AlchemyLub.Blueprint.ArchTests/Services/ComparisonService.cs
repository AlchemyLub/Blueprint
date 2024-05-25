namespace AlchemyLub.Blueprint.ArchTests.Services;

/// <summary>
/// Сервис для полных структурных сравнений
/// </summary>
internal static class StructuralComparisonService
{
    /// <summary>
    /// Структурно сравнивает два любых типа
    /// </summary>
    /// <param name="firstType">Первый тип</param>
    /// <param name="secondType">Второй тип</param>
    /// <returns>Агрегированный результат проверки</returns>
    /// <exception cref="InvalidEnumArgumentException">Ошибка возникает, если невозможно определить упрощённый тип сравниваемых объектов</exception>
    internal static AssertResult CompareTypes(Type firstType, Type secondType)
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
            SimplifiedType.Enum => CompareEnumTypes(firstType, secondType),
            SimplifiedType.CustomType => CompareTypes(firstType, secondType),
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
    internal static AssertResult CompareContracts(Type firstContractType, Type secondContractType)
    {
        AssertResult result = new();

        MethodInfo[] controllerMethods = firstContractType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t.DeclaringType == firstContractType && !t.CheckGeneratedAttributes())
            .ToArray();
        MethodInfo[] contractMethods = secondContractType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t.DeclaringType == secondContractType && !t.CheckGeneratedAttributes())
            .ToArray();

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
    /// <param name="firstMethodInfo">Первый метод</param>
    /// <param name="secondMethodInfo">Второй метод</param>
    /// <returns>Агрегированный результат проверки</returns>
    internal static AssertResult CompareMethods(MethodInfo firstMethodInfo, MethodInfo secondMethodInfo)
    {
        AssertResult result = new();

        Type firstReturnType = firstMethodInfo.ReturnType;
        Type secondReturnType = secondMethodInfo.ReturnType;

        if (firstReturnType != secondReturnType)
        {
            result.Combine(CompareTypes(firstReturnType, secondReturnType));
        }

        ParameterInfo[] firstParameters = firstMethodInfo.GetParameters();
        ParameterInfo[] secondParameters = secondMethodInfo.GetParameters();

        if (firstParameters.Length != secondParameters.Length)
        {
            return result.AddError($"У методов [{firstMethodInfo.Name}] и [{secondMethodInfo.Name}] не совпадает количество параметров");
        }

        if (firstParameters.Length > 1)
        {
            Array.Sort(firstParameters, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(secondParameters, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < firstParameters.Length; i++)
        {
            ParameterInfo firstParameterInfo = firstParameters[i];
            ParameterInfo secondParameterInfo = secondParameters[i];

            string firstParameterInfoName = firstParameterInfo.Name!;
            string secondParameterInfoName = secondParameterInfo.Name!;

            if (firstParameterInfoName != secondParameterInfoName)
            {
                result.AddError($"Параметры [{firstParameterInfoName}] и [{secondParameterInfoName}] имеют разные имена");
            }

            result.Combine(CompareTypes(firstParameterInfo.ParameterType, secondParameterInfo.ParameterType));
        }

        return result;
    }

    /// <summary>
    /// Структурно сравнивает два типа перечисления [<see langword="enum"/>]
    /// </summary>
    /// <param name="firstEnum">Первый тип перечисления [<see langword="enum"/>]</param>
    /// <param name="secondEnum">Второй тип перечисления [<see langword="enum"/>]</param>
    /// <returns>Агрегированный результат проверки</returns>
    internal static AssertResult CompareEnumTypes(Type firstEnum, Type secondEnum)
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

        FieldInfo[] firstFieldsInfo = firstEnum.GetFields();
        FieldInfo[] secondFieldsInfo = secondEnum.GetFields();

        if (firstFieldsInfo.Length != secondFieldsInfo.Length)
        {
            return result.AddError($"У перечислений [{firstEnum.FullName}] и [{secondEnum.FullName}] не совпадает количество значений");
        }

        if (firstFieldsInfo.Length > 1)
        {
            Array.Sort(firstFieldsInfo, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(secondFieldsInfo, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < firstFieldsInfo.Length; i++)
        {
            FieldInfo firstFieldInfo = firstFieldsInfo[i];
            FieldInfo secondFieldInfo = secondFieldsInfo[i];

            string firstFieldInfoName = firstFieldInfo.Name;
            string secondFieldInfoName = firstFieldInfo.Name;

            object? firstFieldInfoConstantValue = firstFieldInfo.GetRawConstantValue();
            object? secondFieldInfoConstantValue = secondFieldInfo.GetRawConstantValue();

            if (firstFieldInfoName != secondFieldInfoName || firstFieldInfoConstantValue != secondFieldInfoConstantValue)
            {
                result.AddError($"У перечислений [{firstEnum.FullName}] и [{secondEnum.FullName}] не совпадают значения " +
                                $"{firstFieldInfoConstantValue}. {firstFieldInfoName}" +
                                $" <-> {secondFieldInfoConstantValue}. {secondFieldInfoName}");
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
    internal static AssertResult CompareCustomTypes(Type firstCustomType, Type secondCustomType)
    {
        AssertResult result = new();

        PropertyInfo[] firstTypeProperties = firstCustomType.GetProperties();
        PropertyInfo[] secondTypeProperties = secondCustomType.GetProperties();

        if (firstTypeProperties.Length != secondTypeProperties.Length)
        {
            return result.AddError($"У типов [{firstCustomType.FullName}] и [{secondCustomType.FullName}] не совпадает количество свойств");
        }

        if (firstTypeProperties.Length > 0)
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
}
