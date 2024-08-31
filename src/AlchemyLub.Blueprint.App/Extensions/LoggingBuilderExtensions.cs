using OpenTelemetry.Logs;

namespace AlchemyLub.Blueprint.App.Extensions;

/// <summary>
/// Методы расширения для <see cref="ILoggingBuilder"/>
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Подключает и настраивает наблюдаемость к логгированию
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static ILoggingBuilder AddObservability(this ILoggingBuilder services) =>
        services
            .AddOpenTelemetry(options =>
                options
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: "ServiceName",
                        serviceVersion: "1.0.0"))
                    .AddOtlpExporter());
}
