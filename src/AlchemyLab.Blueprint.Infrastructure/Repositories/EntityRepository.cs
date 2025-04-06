namespace AlchemyLab.Blueprint.Infrastructure.Database.Repositories;

/// <inheritdoc cref="IEntityRepository"/>
public class EntityRepository(IOptionsSnapshot<CacheOptions> cacheOptions) : IEntityRepository
{
    private readonly CacheOptions inMemoryCache = cacheOptions.GetMemoryCacheOptions();

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

        CacheOptions cache = inMemoryCache;

        if (cache.IsAbsoluteExpiration)
        {
            await Task.CompletedTask;
        }

        return defaultEntityFunc(id);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateEntity()
    {
        await Task.CompletedTask;

        return defaultEntityFunc(Guid.NewGuid()).Id;
    }

    /// <inheritdoc />
    public async Task DeleteEntity(Guid id)
    {
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<Entity> UpdateEntity(Entity entity)
    {
        await Task.CompletedTask;

        return entity;
    }
}
