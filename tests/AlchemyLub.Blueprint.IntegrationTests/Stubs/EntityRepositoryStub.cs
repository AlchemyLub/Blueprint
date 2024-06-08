namespace AlchemyLub.Blueprint.IntegrationTests.Stubs;

/// <summary>
/// Заглушка для <see cref="IEntityRepository"/>
/// </summary>
public class EntityRepositoryStub : IEntityRepository
{
    private readonly Func<Guid, Entity> defaultEntityFunc = id => new(id)
    {
        Title = "Entity title stub",
        Description = "Entity description stub",
        CreatedAt = DateTime.UtcNow
    };

    /// <inheritdoc />
    public async Task<Entity> GetEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntityFunc(id);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateEntity()
    {
        await Task.CompletedTask;

        return defaultEntityFunc(Guid.NewGuid()).Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteEntity(Guid id)
    {
        await Task.CompletedTask;

        return true;
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
