namespace AlchemyLub.Blueprint.Application.Services;

/// <inheritdoc cref="IApplicationService"/>
internal sealed class ApplicationService(IInfrastructureService infrastructureService) : IApplicationService
{
    /// <inheritdoc />
    public async Task<EntityResponse> GetEntity(Guid id)
    {
        Entity entity = await infrastructureService.GetDbEntity(id);

        return new(entity.Id, entity.Title);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateEntity(EntityType entityType) =>
        await infrastructureService.CreateDbEntity();

    /// <inheritdoc />
    public async Task<bool> DeleteEntity(Guid id) => await infrastructureService.DeleteDbEntity(id);

    /// <inheritdoc />
    public async Task<EntityResponse> UpdateEntity(Guid id, EntityRequest request)
    {
        Entity entity = await infrastructureService.UpdateDbEntity(new(id)
        {
            Title = request.Title,
            Description = request.Description
        });

        return new(entity.Id, entity.Title);
    }
}
