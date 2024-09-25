namespace AlchemyLub.Blueprint.Infrastructure.Database.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы инфраструктурного слоя
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddDatabaseInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabaseContext(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped<IEntityRepository, EntityRepository>();

    private static IServiceCollection AddDatabaseContext(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>((serviceProvider, optionBuilder) =>
        {
            optionBuilder.UseNpgsql(
                configuration.GetPostgreSqlConnectionString(),
                builder => builder.EnableRetryOnFailure(
                    PostgreSqlValues.MaxRetryCount,
                    PostgreSqlValues.MaxRetryDelay,
                    null));

            optionBuilder.LogTo(
                (eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                eventData =>
                {
                    if (eventData is ExecutionStrategyEventData executionStrategyEventData)
                    {
                        ILogger<ApplicationDbContext> logger =
                            serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

                        IReadOnlyList<Exception> exceptions = executionStrategyEventData.ExceptionsEncountered;

                        string message = exceptions[^1].Message;

                        logger.LogWarning(
                            "Retry [{Count}] with delay [{Delay}] due to error: {Message}",
                            exceptions.Count,
                            executionStrategyEventData.Delay,
                            message);
                    }
                });
        });
}
