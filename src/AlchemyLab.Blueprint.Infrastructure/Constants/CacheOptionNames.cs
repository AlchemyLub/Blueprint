namespace AlchemyLab.Blueprint.Infrastructure.Database.Constants;

/// <summary>
/// Константные имена настроек кеша
/// </summary>
public static class CacheOptionNames
{
    /// <summary>
    /// Кеш в памяти
    /// </summary>
    public const string MemoryCache = nameof(MemoryCache);

    /// <summary>
    /// Распределённый кеш
    /// </summary>
    public const string DistributedCache = nameof(DistributedCache);

    /// <summary>
    /// Кеш в памяти для идемпотентных запросов
    /// </summary>
    public const string IdempotencyMemoryCache = nameof(IdempotencyMemoryCache);

    /// <summary>
    /// Распределённый кеш для идемпотентных запросов
    /// </summary>
    public const string IdempotencyDistributedCache = nameof(IdempotencyDistributedCache);
}
