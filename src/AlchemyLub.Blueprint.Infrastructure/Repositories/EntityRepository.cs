namespace AlchemyLub.Blueprint.Infrastructure.Repositories;

/// <inheritdoc cref="IEntityRepository"/>
public class EntityRepository : IEntityRepository
{
    private readonly Func<Guid, Entity> defaultEntityFunc = id => new(id)
    {
        Title = "Entity title",
        Description = "Entity description",
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
    public async Task<Result> DeleteEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntityFunc(id).Id == id
            ? Result.Success()
            : Result.Failure(new("Entity not found"));
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
