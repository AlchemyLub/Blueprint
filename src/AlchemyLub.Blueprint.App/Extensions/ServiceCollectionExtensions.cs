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
            .AddApplicationLayer()
            .AddEndpointsLayer()
            .AddInfrastructureLayer(configuration)
            .AddMiddlewares();

    private static IServiceCollection AddMiddlewares(this IServiceCollection services) =>
        services
            .AddScoped<RequestContextLoggingMiddleware>();
}
