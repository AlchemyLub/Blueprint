namespace AlchemyLub.Blueprint.ArchTests;

// TODO: Дописать по мере возможностей! Пока готово только сравнение типов(надо ещё проверить на дженериках) и сравнить все методы их параметры и возвращаемые значения
/// <summary>
/// Тесты для проверки корректного сопоставления контрактов и конечных точек
/// </summary>
public class ContractsTests
{
    [Fact]
    public void TestContractsCorrespondToControllers()
    {
        //IEnumerable<Type> controllerTypes = Assemblies.EndpointsAssembly.GetTypes().Where(t =>
        //    typeof(ControllerBase).IsAssignableFrom(t)
        //    && t.Name.EndsWith(TypeNameSuffixes.Controller, StringComparison.InvariantCultureIgnoreCase));
        //IEnumerable<Type> clientTypes = Assemblies.ClientsAssembly.GetTypes().Where(t =>
        //    t.Name.EndsWith(TypeNameSuffixes.Client, StringComparison.InvariantCultureIgnoreCase)
        //    && t.IsInterface);

        //IEnumerable<(Type controllerType, Type clientType)> equaledTypes = from controllerType in controllerTypes
        //    join clientType
        //        in clientTypes
        //        on controllerType.Name[..^(TypeNameSuffixes.Controller.Length - 1)]
        //        equals clientType.Name[1..^(TypeNameSuffixes.Client.Length - 1)]
        //    select (controllerType, clientType);

        AssertResult result = new();

        //foreach ((Type ControllerType, Type ClientType) in equaledTypes)
        //{
        //    result = result.Combine(EqualTypes(ControllerType, ClientType));
        //}

        Type controllerType = typeof(EntitiesController);
        Type contractType = typeof(EntitiesController);

        MethodInfo[] controllerMethods = controllerType.GetMethods();
        MethodInfo[] contractMethods = contractType.GetMethods();

        // TODO: Исключение для неравного количества

        if (controllerMethods.Length > 1)
        {
            Array.Sort(controllerMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(contractMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        result = result.Combine(EqualMethods(typeof(EntityRequest), typeof(ClientEntityRequest)));

        if (!result.IsSuccessful)
        {
            foreach (string error in result.GetErrors())
            {
                Console.WriteLine(error);
            }
        }

        result.IsSuccessful.Should().BeTrue();
    }


    private AssertResult EqualMethods(MethodInfo firstMethodInfo, MethodInfo secondMethodInfo)
    {
        AssertResult result = new();

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

            result = result.Combine(EqualTypes(firstParameterInfo.ParameterType, secondParameterInfo.ParameterType));
        }

        return result;
    }


    private AssertResult EqualTypes(Type firstType, Type secondType)
    {
        AssertResult result = new();

        if (firstType == secondType)
        {
            return result;
        }

        if (!TryCheckTestTypes(firstType, secondType, out TestType? testType))
        {
            return result.AddError($"Типы [{firstType.FullName}] и [{secondType.FullName}] не соответствуют друг другу");
        }

        return testType switch
        {
            TestType.Primitive => result.AddError($"Типы {firstType.FullName} и {secondType.FullName}" +
                                                  $" являются базовыми и не соответствуют друг другу"),
            TestType.Enum => CheckEnumTypes(firstType, secondType),
            TestType.CustomType => CheckCustomTypes(firstType, secondType),
            _ => throw new InvalidEnumArgumentException(nameof(testType), (int)testType, typeof(TestType))
        };
    }

    private AssertResult CheckEnumTypes(Type firstType, Type secondType)
    {
        AssertResult result = new();

        FieldInfo[] firstFieldsInfo = firstType.GetFields();
        FieldInfo[] secondFieldsInfo = secondType.GetFields();

        if (firstFieldsInfo.Length != secondFieldsInfo.Length)
        {
            return result.AddError($"У перечислений [{firstType.FullName}] и [{secondType.FullName}] не совпадает количество значений");
        }

        Array.Sort(firstFieldsInfo, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        Array.Sort(secondFieldsInfo, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));

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
                result.AddError($"У перечислений [{firstType.FullName}] и [{secondType.FullName}] не совпадают значения " +
                                $"{firstFieldInfoConstantValue}. {firstFieldInfoName}" +
                                $" <-> {secondFieldInfoConstantValue}. {secondFieldInfoName}");
            }
        }

        return result;
    }

    private AssertResult CheckCustomTypes(Type firstType, Type secondType)
    {
        AssertResult result = new();

        PropertyInfo[] firstTypeProperties = firstType.GetProperties();
        PropertyInfo[] secondTypeProperties = secondType.GetProperties();

        if (firstTypeProperties.Length != secondTypeProperties.Length)
        {
            return result.AddError($"У типов [{firstType.FullName}] и [{secondType.FullName}] не совпадает количество свойств");
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
                    $"У типов [{firstType.FullName}] и [{secondType.FullName}] свойства не совпадают по именам. " +
                    $"[{firstPropertyInfo.Name} != {secondPropertyInfo.Name}]");
            }

            Type firstPropertyType = firstTypeProperties[i].PropertyType;
            Type secondPropertyType = secondTypeProperties[i].PropertyType;

            AssertResult comparingTypesResult = EqualTypes(firstPropertyType, secondPropertyType);

            result.Combine(comparingTypesResult);
        }

        return result;
    }

    private bool TryCheckTestTypes(Type firstType, Type secondType, [NotNullWhen(true)] out TestType? testType)
    {
        TestType firstTestType = GetTestType(firstType);
        TestType secondTestType = GetTestType(secondType);

        if (firstTestType == secondTestType)
        {
            testType = firstTestType;
            return true;
        }

        testType = null;
        return false;
    }

    private TestType GetTestType(Type type)
    {
        if (type.IsEnum)
        {
            return TestType.Enum;
        }

        return IsSimpleType(type)
            ? TestType.Primitive
            : TestType.CustomType;
    }

    private bool IsSimpleType(Type type)
    {
        HashSet<Type> primitiveTypes = new()
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateOnly),
            typeof(TimeOnly),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        if (type.IsPrimitive || primitiveTypes.Contains(type))
        {
            return true;
        }

        return TryUnboxNullableType(type, out Type? unboxType) && IsSimpleType(unboxType);
    }

    private bool TryUnboxNullableType(Type type, [NotNullWhen(true)] out Type? unboxNullableType)
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

    /// <summary>
    /// Упрощённая типизация для тестирования
    /// </summary>
    private enum TestType
    {
        /// <summary>
        /// Неизвестный тип
        /// </summary>
        /// <remarks>
        /// Указывает на исключительный случай, в корректно работающем коде не должен попадаться
        /// </remarks>
        Unknown = 0,

        /// <summary>
        /// Примитивный тип, находящийся в BCL
        /// </summary>
        /// <remarks>
        /// <b>Необходимые действия:</b> просто сравнить типы
        /// </remarks>
        Primitive = 1,

        /// <summary>
        /// Тип перечисление
        /// </summary>
        /// <remarks>
        /// <b>Необходимые действия:</b> сравнить все значения перечисления
        /// </remarks>
        Enum = 2,

        /// <summary>
        /// Пользовательский тип
        /// </summary>
        /// <remarks>
        /// <b>Необходимые действия:</b> сравнить все свойства типа
        /// </remarks>
        CustomType = 3
    }
}
