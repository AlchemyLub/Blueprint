namespace FloralHub.Blueprint.Application.Services;

/// <inheritdoc cref="IApplicationService"/>
internal sealed class ApplicationService : IApplicationService
{
    /// <inheritdoc />
    public async Task<IEntity> GetEntityAsync(Guid id) => await Task.Run(() => new Entity { Id = id });
}
