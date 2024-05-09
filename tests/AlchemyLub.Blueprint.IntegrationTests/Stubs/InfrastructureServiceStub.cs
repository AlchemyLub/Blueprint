namespace AlchemyLub.Blueprint.IntegrationTests.Stubs;

/// <summary>
/// Заглушка для <see cref="IInfrastructureService"/>
/// </summary>
public class InfrastructureServiceStub : IInfrastructureService
{
    /// <inheritdoc />
    public async Task<Entity> GetDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return new Entity(id) { Title = "Title Stub" };
    }

    /// <inheritdoc />
    public async Task<Guid> CreateDbEntity()
    {
        await Task.CompletedTask;

        Entity entity = new Entity(Guid.NewGuid()) { Title = "Title Stub" };

        return entity.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return true;
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateDbEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
