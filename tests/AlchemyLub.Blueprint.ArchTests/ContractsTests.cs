using static AlchemyLub.Blueprint.ArchTests.Constants;

namespace AlchemyLub.Blueprint.ArchTests;

// TODO: Дописать по мере возможностей
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
                join clientType in clientTypes on controllerType.Name equals clientType.Name[1..]
                select (controllerType, clientType);



        foreach (Type controllerType in controllerTypes)
        {
            string str = $"I{controllerType.Name.Replace("Controller", "Client")}";

            // Get the corresponding contract interface
            Type? contractInterface = controllerType.GetInterface(str, true);

            // Check if interface exists
            contractInterface.Should().NotBeNull();

            // Get all methods in the controller
            MethodInfo[] controllerMethods = controllerType.GetMethods();

            // Get all methods in the contract interface
            MethodInfo[] contractMethods = contractInterface!.GetMethods();

            foreach (MethodInfo controllerMethod in controllerMethods)
            {
                // Check if the method exists in the contract interface
                MethodInfo? correspondingContractMethod = contractMethods.FirstOrDefault(m => m.Name == controllerMethod.Name);
                correspondingContractMethod.Should().NotBeNull();

                // Check if the method signature matches
                controllerMethod.GetParameters().Should().HaveCount(correspondingContractMethod!.GetParameters().Length);
                controllerMethod.ReturnType.Should().Be(correspondingContractMethod.ReturnType);
            }
        }
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

    // TODO: Сгенерировано через GPT, надо подстроить под себя.
    private static bool CompareClasses(string controllerClassName, string contractClassName)
    {
        Assembly controllerAssembly = Assembly.LoadFile(controllerClassName);
        Assembly contractAssembly = Assembly.LoadFile(contractClassName);

        Type? controllerType = controllerAssembly.GetTypes().FirstOrDefault(t => t.Name == "MyControllerClass");
        Type? contractType = contractAssembly.GetTypes().FirstOrDefault(t => t.Name == "MyContractClass");

        if (controllerType == null || contractType == null)
        {
            Console.WriteLine("Controller or Contract class not found");
            return false;
        }

        foreach (MethodInfo controllerMethod in controllerType.GetMethods())
        {
            MethodInfo? contractMethod = contractType.GetMethod(controllerMethod.Name);
            if (contractMethod == null)
            {
                Console.WriteLine($"Method {controllerMethod.Name} not found in the Contract class");
                return false;
            }

            if (!CheckMethodCompliance(controllerMethod, contractMethod))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CheckMethodCompliance(MethodInfo controllerMethod, MethodInfo contractMethod)
    {
        ParameterInfo[] controllerParameters = controllerMethod.GetParameters();
        ParameterInfo[] contractParameters = contractMethod.GetParameters();
        Type controllerReturnType = controllerMethod.ReturnType;
        Type contractReturnType = contractMethod.ReturnType;

        if (!CheckTypeEquality(controllerReturnType, contractReturnType))
        {
            Console.WriteLine($"Return types do not match for method: {controllerMethod.Name}");
            return false;
        }

        for (int i = 0; i < controllerParameters.Length; i++)
        {
            if (!CheckTypeEquality(controllerParameters[i].ParameterType, contractParameters[i].ParameterType))
            {
                Console.WriteLine($"Parameter types do not match for method: {controllerMethod.Name}");
                return false;
            }
        }

        return true;
    }

    public static bool CheckTypeEquality(Type type1, Type type2)
    {
        if ((type1.IsPrimitive && type2.IsPrimitive) || type1 == type2)
        {
            return true;
        }
        else if (type1.IsClass && type2.IsClass)
        {
            PropertyInfo[] type1Properties = type1.GetProperties();
            PropertyInfo[] type2Properties = type2.GetProperties();

            if (type1Properties.Length != type2Properties.Length)
            {
                return false;
            }

            for (int i = 0; i < type1Properties.Length; i++)
            {
                if (type1Properties[i].Name != type2Properties[i].Name || !CheckTypeEquality(type1Properties[i].PropertyType, type2Properties[i].PropertyType))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
