namespace AlchemyLub.Blueprint.ArchTests;

/// <summary>
/// Тесты для проверки корректного сопоставления контрактов и конечных точек
/// </summary>
public class ContractsTests
{
    [Fact]
    public void TestContractsCorrespondToControllers()
    {
        AssertResult result = new();
        Type[] controllerTypes = Assemblies.EndpointsAssembly
            .GetTypes()
            .Where(t =>
                typeof(ControllerBase).IsAssignableFrom(t)
                && t.Name.EndsWith(TypeNameSuffixes.Controller, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();

        Type[] clientTypes = Assemblies.ClientsAssembly
            .GetTypes()
            .Where(t => t.Name.EndsWith(TypeNameSuffixes.Client, StringComparison.InvariantCultureIgnoreCase) && !t.IsInterface)
            .ToArray();

        if (controllerTypes.Length != clientTypes.Length)
        {
            result.AddError("Не совпадает количество контроллеров и клиентов");
        }

        if (controllerTypes.Length > 1)
        {
            Array.Sort(controllerTypes, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(clientTypes, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < controllerTypes.Length; i++)
        {
            result.Combine(EqualContractClasses(controllerTypes[i], clientTypes[i]));
        }

        result.IsSuccessful.Should().BeTrue();
    }

    private AssertResult EqualContractClasses(Type controllerType, Type contractType)
    {
        AssertResult result = new();

        MethodInfo[] controllerMethods = controllerType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t.DeclaringType == controllerType && !t.CheckGeneratedAttributes())
            .ToArray();
        MethodInfo[] contractMethods = contractType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t.DeclaringType == contractType && !t.CheckGeneratedAttributes())
            .ToArray();

        if (controllerMethods.Length != contractMethods.Length)
        {
            result.AddError($"У классов [{controllerType.FullName}] и [{contractType.FullName}] не совпадает количество методов");
        }

        if (controllerMethods.Length > 1)
        {
            Array.Sort(controllerMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
            Array.Sort(contractMethods, (p1, p2) => string.CompareOrdinal(p1.Name, p2.Name));
        }

        for (int i = 0; i < controllerMethods.Length; i++)
        {
            result = result.Combine(StructuralComparisonService.CompareMethods(controllerMethods[i], contractMethods[i]));
        }

        return result;
    }
}
