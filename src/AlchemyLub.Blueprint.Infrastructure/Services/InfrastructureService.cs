namespace AlchemyLub.Blueprint.Infrastructure.Services;

/// <inheritdoc cref="IInfrastructureService"/>
public sealed class InfrastructureService : IInfrastructureService
{
    private readonly Func<Guid, Entity> defaultEntityFunc = id => new(id)
    {
        Title = "Entity title",
        Description = "Entity description",
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

        return defaultEntityFunc(id).Id == id;
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateDbEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
