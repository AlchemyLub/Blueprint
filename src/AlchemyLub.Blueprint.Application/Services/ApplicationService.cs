namespace AlchemyLub.Blueprint.Application.Services;

/// <inheritdoc cref="IApplicationService"/>
internal sealed class ApplicationService(IInfrastructureService infrastructureService) : IApplicationService
{
    /// <inheritdoc />
    public async Task<IEntity> GetEntity(Guid id)
    {
        IEntity entity = await infrastructureService.GetDbEntity(id);

        return entity;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateEntity(EntityType entityType)
    {
        Guid entityId = await infrastructureService.AddDbEntity();

        return entityId;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteEntity(Guid id) => await infrastructureService.DeleteDbEntity(id);
}
