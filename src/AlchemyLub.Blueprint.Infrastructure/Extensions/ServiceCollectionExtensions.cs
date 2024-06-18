namespace AlchemyLub.Blueprint.Infrastructure.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы инфраструктурного слоя
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services) =>
        services
            .AddServices()
            .AddDatabaseContext();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped<IEntityRepository, EntityRepository>();

    private static IServiceCollection AddDatabaseContext(this IServiceCollection services) =>
        services
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {

            });
}
