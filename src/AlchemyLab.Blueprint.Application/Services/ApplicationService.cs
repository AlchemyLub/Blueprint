namespace AlchemyLab.Blueprint.Application.Services;

/// <inheritdoc cref="IApplicationService"/>
internal sealed class ApplicationService(IEntityRepository repository) : IApplicationService
{
    /// <inheritdoc />
    public async Task<Entity> GetEntity(Guid id) => await repository.GetEntity(id);

    /// <inheritdoc />
    public async Task<Guid> CreateEntity() => await repository.CreateEntity();

    /// <inheritdoc />
    public async Task DeleteEntity(Guid id) => await repository.DeleteEntity(id);

    /// <inheritdoc />
    public async Task<Entity> UpdateEntity(Guid id, Entity request) =>
        await repository.UpdateEntity(new(id)
        {
            Title = request.Title,
            Description = request.Description
        });
}
