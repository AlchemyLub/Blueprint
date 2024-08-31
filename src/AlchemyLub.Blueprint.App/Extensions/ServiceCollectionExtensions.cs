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

    /// <summary>
    /// Регистрирует наблюдаемость для приложения
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="serviceName">Имя сервиса для которого подключается наблюдаемость</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddObservability(this IServiceCollection services, string serviceName)
    {
        OpenTelemetryBuilder openTelemetry = services.AddOpenTelemetry();

        openTelemetry.ConfigureResource(res => res.AddService(serviceName));

        openTelemetry.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            // tracing.AddSource() // TODO: Разобраться что это такое
            
        });

        openTelemetry.WithMetrics(metrics =>
        {
            metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));

            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddRuntimeInstrumentation();

            metrics.AddPrometheusExporter();
        });

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter())
            .WithMetrics(metrics =>
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter());

        return services;
    }

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
