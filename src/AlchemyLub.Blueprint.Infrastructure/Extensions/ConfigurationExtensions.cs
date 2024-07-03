namespace AlchemyLub.Blueprint.Infrastructure.Database.Extensions;

/// <summary>
/// Provides extension methods for <inheritdoc cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets the PostgreSQL connection string from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The PostgreSQL connection string, or null if not found.</returns>
    public static string? GetPostgreSqlConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString(PostgreSqlConstants.Name);

    /// <summary>
    /// Gets the Redis connection string from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The Redis connection string, or null if not found.</returns>
    public static string? GetRedisConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString(RedisConstants.Name);
}
