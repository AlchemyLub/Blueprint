namespace AlchemyLub.Blueprint.ArchTests;

/// <summary>
/// Тесты для проверки корректного сопоставления контрактов и конечных точек
/// </summary>
public class ContractsTests
{
    // TODO: Сделать отдельный корректный тест
    [Fact]
    public void TestContractsCorrespondToControllers()
    {
        IEnumerable<Type> controllers = Assemblies.ClientsAssembly.GetAllControllers();

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
            result.Combine(StructuralComparisonService.CompareContracts(controllerTypes[i], clientTypes[i]));
        }

        result.IsSuccessful.Should().BeTrue();
    }
}
