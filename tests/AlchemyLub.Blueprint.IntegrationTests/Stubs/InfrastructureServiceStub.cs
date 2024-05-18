namespace AlchemyLub.Blueprint.IntegrationTests.Stubs;

/// <summary>
/// Заглушка для <see cref="IInfrastructureService"/>
/// </summary>
public class InfrastructureServiceStub : IInfrastructureService
{
    private readonly Func<Guid, Entity> defaultEntityFunc = id => new(id)
    {
        Title = "Entity title stub",
        Description = "Entity description stub",
        CreatedAt = DateTime.UtcNow
    };

    /// <inheritdoc />
    public async Task<Entity> GetDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntityFunc(id);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateDbEntity()
    {
        await Task.CompletedTask;

        return defaultEntityFunc(Guid.NewGuid()).Id;
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
