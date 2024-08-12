namespace AlchemyLub.Blueprint.App.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все слои приложения
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAllLayers(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddOptions()
            .AddApplicationLayer()
            .AddEndpointsLayer()
            .AddInfrastructureLayer(configuration)
            .AddMiddlewares();

    private static IServiceCollection AddMiddlewares(this IServiceCollection services) =>
        services
            .AddScoped<RequestContextLoggingMiddleware>();

    private static IServiceCollection AddOptions(this IServiceCollection services)
    {
        services.AddOptionsWithValidation<CacheOptions, CacheOptionsValidator>(CacheOptionNames.MemoryCache);
        services.AddOptionsWithValidation<CacheOptions, CacheOptionsValidator>(CacheOptionNames.DistributedCache);
        services.AddOptionsWithValidation<CacheOptions, CacheOptionsValidator>(CacheOptionNames.IdempotencyMemoryCache);
        services.AddOptionsWithValidation<CacheOptions, CacheOptionsValidator>(CacheOptionNames.IdempotencyDistributedCache);

        return services;
    }
}
