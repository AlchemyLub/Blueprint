namespace AlchemyLab.Blueprint.Infrastructure.Database.Enums;

/// <summary>
/// Represents the type of cache store.
/// </summary>
public enum CacheStore
{
    /// <summary>
    /// No cache store is used
    /// </summary>
    None = 0,

    /// <summary>
    /// Кеш в памяти
    /// </summary>
    InMemory = 1,

    /// <summary>
    /// Распределённый кеш
    /// </summary>
    Distributed = 2
}
