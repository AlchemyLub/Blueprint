namespace AlchemyLub.Blueprint.Infrastructure.Options;

/// <summary>
/// Represents the options for caching.
/// </summary>
public sealed class CacheOptions
{
    /// <summary>
    /// Value indicating whether caching is enabled.
    /// </summary>
    public required bool IsEnabled { get; set; }

    /// <summary>
    /// Type of cache store to use.
    /// </summary>
    public required CacheStore CacheStore { get; set; }

    /// <summary>
    /// Duration for which the cache is valid.
    /// </summary>
    public required int CacheDuration { get; set; }

    /// <summary>
    /// Value indicating whether the cache should use sliding expiration.
    /// </summary>
    public required bool IsSlidingExpiration { get; set; }

    /// <summary>
    /// Value indicating whether the cache should use absolute expiration.
    /// </summary>
    public required bool IsAbsoluteExpiration { get; set; }

    /// <summary>
    /// Value indicating whether to cache null values.
    /// </summary>
    public required bool CacheNullValues { get; set; }
}
