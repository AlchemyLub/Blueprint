namespace AlchemyLub.Blueprint.Infrastructure.Database.Enums;

/// <summary>
/// Represents the type of cache store.
/// </summary>
public enum CacheStore
{
    /// <summary>
    /// No cache store is used.
    /// </summary>
    None = 0,

    /// <summary>
    /// In-memory cache is used as the cache store.
    /// </summary>
    InMemory = 1,

    /// <summary>
    /// Redis is used as the cache store.
    /// </summary>
    Redis = 2,

    /// <summary>
    /// Garnet is used as the cache store.
    /// </summary>
    Garnet = 3
}
