namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы необходимые для реализации идемпотентности
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddIdempotencyServices(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Более детально настроить кеши
        services.AddMemoryCache(options =>
        {
            options.TrackStatistics = true;
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetRedisConnectionString();
        });

        return services;
    }
}
