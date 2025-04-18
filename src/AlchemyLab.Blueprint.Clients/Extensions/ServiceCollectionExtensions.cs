using AlchemyLab.Blueprint.Clients.Abstractions;

namespace AlchemyLab.Blueprint.Clients.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все клиенты сервиса <see cref="AlchemyLab.Blueprint"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddBlueprintClients(this IServiceCollection services)
    {
        services.AddRefitClient<IEntitiesClient>();

        return services;
    }
}
