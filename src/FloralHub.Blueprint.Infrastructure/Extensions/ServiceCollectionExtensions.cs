namespace FloralHub.Blueprint.Infrastructure.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services) =>
        services.AddScoped<IInfrastructureService, InfrastructureService>();
}
