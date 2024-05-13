namespace AlchemyLub.Blueprint.Infrastructure.Services;

/// <inheritdoc cref="IInfrastructureService"/>
public sealed class InfrastructureService : IInfrastructureService
{
    private readonly Entity defaultEntity = new(Guid.NewGuid())
    {
        Title = "Entity title",
        Description = "Entity description",
        CreatedAt = DateTime.UtcNow
    };

    /// <inheritdoc />
    public async Task<Entity> GetDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntity;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateDbEntity()
    {
        await Task.CompletedTask;

        return defaultEntity.Id;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntity.Id == id;
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateDbEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
