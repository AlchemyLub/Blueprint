namespace AlchemyLub.Blueprint.IntegrationTests.Stubs;

/// <summary>
/// Заглушка для <see cref="IInfrastructureService"/>
/// </summary>
public class InfrastructureServiceStub : IInfrastructureService
{
    /// <inheritdoc />
    public async Task<IEntity> GetDbEntityAsync(Guid id) => await Task.Run(() => new Entity(id) { Title = "Title Stub" });
}
