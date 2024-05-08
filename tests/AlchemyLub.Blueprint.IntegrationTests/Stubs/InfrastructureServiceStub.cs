namespace AlchemyLub.Blueprint.IntegrationTests.Stubs;

/// <summary>
/// Заглушка для <see cref="IInfrastructureService"/>
/// </summary>
public class InfrastructureServiceStub : IInfrastructureService
{
    /// <inheritdoc />
    public async Task<IEntity> GetDbEntity(Guid id) => await Task.Run(() => new Entity(id) { Title = "Title Stub" });

    /// <inheritdoc />
    public Task<Guid> AddDbEntity() => throw new NotImplementedException();

    /// <inheritdoc />
    public Task<bool> DeleteDbEntity(Guid id) => throw new NotImplementedException();
}
