namespace AlchemyLub.Blueprint.Application.Services;

/// <inheritdoc cref="IApplicationService"/>
internal sealed class ApplicationService(IInfrastructureService infrastructureService) : IApplicationService
{
    /// <inheritdoc />
    public async Task<Entity> GetEntity(Guid id) => await infrastructureService.GetDbEntity(id);

    /// <inheritdoc />
    public async Task<Guid> CreateEntity() => await infrastructureService.CreateDbEntity();

    /// <inheritdoc />
    public async Task<bool> DeleteEntity(Guid id) => await infrastructureService.DeleteDbEntity(id);

    /// <inheritdoc />
    public async Task<Entity> UpdateEntity(Guid id, Entity request) =>
        await infrastructureService.UpdateDbEntity(new(id)
        {
            Title = request.Title,
            Description = request.Description
        });
}
