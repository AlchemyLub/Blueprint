using static AlchemyLub.Blueprint.ArchTests.Constants.Assemblies;

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
        // Get all controllers in the assembly
        IEnumerable<Type> controllerTypes = EndpointsAssembly.GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t));

        IEnumerable<Type> clientTypes = ClientsAssembly.GetTypes()
            .Where(t => t.Name.EndsWith("Client", StringComparison.InvariantCultureIgnoreCase) && t.IsInterface);

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
}
