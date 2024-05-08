namespace AlchemyLub.Blueprint.Infrastructure.Services;

/// <inheritdoc cref="IInfrastructureService"/>
public sealed class InfrastructureService : IInfrastructureService
{
    private readonly Entity defaultEntity = new(Guid.NewGuid())
    {
        Title = "Entity title"
    };

    /// <inheritdoc />
    public async Task<IEntity> GetDbEntity(Guid id)
    {
        await Task.CompletedTask;

        return defaultEntity;
    }

    /// <inheritdoc />
    public async Task<Guid> AddDbEntity()
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
}
