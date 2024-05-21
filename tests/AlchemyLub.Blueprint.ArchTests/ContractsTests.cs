using static AlchemyLub.Blueprint.ArchTests.Constants;

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
        IEnumerable<Type> controllerTypes = Assemblies.EndpointsAssembly.GetTypes().Where(t =>
            typeof(ControllerBase).IsAssignableFrom(t)
            && t.Name.EndsWith(TypeNameSuffixes.Controller, StringComparison.InvariantCultureIgnoreCase));
        IEnumerable<Type> clientTypes = Assemblies.ClientsAssembly.GetTypes().Where(t =>
            t.Name.EndsWith(TypeNameSuffixes.Client, StringComparison.InvariantCultureIgnoreCase)
            && t.IsInterface);

        IEnumerable<(Type controllerType, Type clientType)> equaledTypes = from controllerType in controllerTypes
            join clientType
                in clientTypes
                on controllerType.Name[..^(TypeNameSuffixes.Controller.Length - 1)]
                equals clientType.Name[1..^(TypeNameSuffixes.Client.Length - 1)]
            select (controllerType, clientType);

        AssertResult result = new();

        //foreach ((Type ControllerType, Type ClientType) in equaledTypes)
        //{
        //    result = result.Combine(EqualTypes(ControllerType, ClientType));
        //}

        result = result.Combine(EqualTypes(typeof(EntityRequest), typeof(ClientEntityRequest)));

        if (!result.IsSuccessful)
        {
            foreach (string error in result.GetErrors())
            {
                Console.WriteLine(error);
            }
        }

        result.IsSuccessful.Should().BeTrue();
    }

    private void AssertContracts(Type controllerType, Type clientType)
    {
        ControllerClientPair pair = new(controllerType, clientType);

        pair.ControllerMethods
            .Should()
            .HaveCount(pair.ClientMethods.Length)
            .And
            .HaveCount(pair.PairedMethods.Length);

        foreach (PairedMethods pairedMethods in pair.PairedMethods)
        {
            pairedMethods.ControllerMethodParameters
                .Should()
                .HaveCount(pairedMethods.ClientMethodParameters.Length)
                .And
                .HaveCount(pairedMethods.ParameterPairedTypes.Length);

            foreach (PairedTypes parameterPairedTypes in pairedMethods.ParameterPairedTypes)
            {
                if (parameterPairedTypes.LeftType == parameterPairedTypes.RightType)
                {
                    continue;
                }

                parameterPairedTypes.LeftType.Should().Be(parameterPairedTypes.RightType);
            }
        }
    }

    private readonly struct ControllerClientPair(Type controllerType, Type clientType)
    {
        public MethodInfo[] ControllerMethods => controllerType.GetMethods();
        public MethodInfo[] ClientMethods => clientType.GetMethods();

        // TODO: Сравнение по имени - не айс, нужен другой вариант.
        public PairedMethods[] PairedMethods =>
            (from controllerMethodInfo in ControllerMethods
             join clientMethodInfo
                 in ClientMethods
                 on controllerMethodInfo.Name
                 equals clientMethodInfo.Name
             select new PairedMethods(controllerMethodInfo, clientMethodInfo))
            .ToArray();
    }

    private readonly struct PairedMethods(MethodInfo controllerMethod, MethodInfo clientMethod)
    {
        public PairedTypes ReturnTypePair => new(controllerMethod.ReturnType, clientMethod.ReturnType);

        public ParameterInfo[] ControllerMethodParameters => controllerMethod.GetParameters();
        public ParameterInfo[] ClientMethodParameters => controllerMethod.GetParameters();

        // TODO: Сравнение по имени - не айс, нужен другой вариант.
        public PairedTypes[] ParameterPairedTypes =>
            (from controllerMethodParameterInfo in ControllerMethodParameters
             join clientMethodParameterInfo
                 in ClientMethodParameters
                 on controllerMethodParameterInfo.Name
                 equals clientMethodParameterInfo.Name
             select new PairedTypes(clientMethodParameterInfo.ParameterType, clientMethodParameterInfo.ParameterType))
            .ToArray();
    }

    private readonly struct PairedTypes(Type leftType, Type rightType)
    {
        public Type LeftType { get; init; } = leftType;
        public Type RightType { get; init; } = rightType;

        public bool IsEquivalent => LeftType == RightType;

        public Lazy<Type> LazyType { get; }
    }

    private AssertResult EqualTypes(Type firstType, Type secondType)
    {
        AssertResult result = new();

        if (firstType == secondType)
        {
            return result;
        }

        var firstTypeProperties = firstType.GetProperties();
        var secondTypeProperties = secondType.GetProperties();

        if (firstTypeProperties.Length != secondTypeProperties.Length)
        {
            return result.AddError($"У типов {firstType.Name} и {secondType.Name} не совпадает количество свойств");
        }

        Array.Sort(firstTypeProperties, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        Array.Sort(secondTypeProperties, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));

        for (int i = 0; i < firstTypeProperties.Length; i++)
        {
            PropertyInfo firstPropertyInfo = firstTypeProperties[i];
            PropertyInfo secondPropertyInfo = secondTypeProperties[i];

            if (firstPropertyInfo.Name != secondPropertyInfo.Name)
            {
                result.AddError(
                    $"У типов {firstType.Name} и {secondType.Name} свойства под индексами {i} не совпадают по именам. " +
                    $"[{firstPropertyInfo.Name} != {secondPropertyInfo.Name}]");
            }

            result = result.Combine(EqualTypes(firstTypeProperties[i].PropertyType, secondTypeProperties[i].PropertyType));
        }

        return result;
    }

    private readonly struct AssertResult()
    {
        public bool IsSuccessful => Errors.Count == 0;

        private List<string> Errors { get; } = new();

        public AssertResult AddError(string errorMessage)
        {
            Errors.Add(errorMessage);

            return this;
        }

        public List<string> GetErrors() => Errors;

        public AssertResult Combine(AssertResult assertResult)
        {
            var errors = assertResult.GetErrors();

            if (errors.Count > 0)
            {
                Errors.AddRange(errors);
            }

            return this;
        }
    }
}
