namespace AlchemyLub.Blueprint.Infrastructure.Services;

/// <inheritdoc cref="IInfrastructureService"/>
public sealed class InfrastructureService : IInfrastructureService
{
    /// <inheritdoc />
    public async Task<IEntity> GetDbEntityAsync(Guid id) => await Task.Run(() => new Entity(id) { Title = "Title" });
}
