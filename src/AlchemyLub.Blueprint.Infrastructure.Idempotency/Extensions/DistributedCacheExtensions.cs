namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Extensions;

/// <summary>
/// Методы расширения для <see cref="IDistributedCache"/>
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// Настройки сериализации JSON, используемые при сохранении и получении объектов из кэша.
    /// </summary>
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Сохраняет значение в кэш с указанным ключом.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого значения.</typeparam>
    /// <param name="cache"><see cref="IDistributedCache"/></param>
    /// <param name="key">Ключ для сохранения значения.</param>
    /// <param name="value">Значение для сохранения в кэше.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Асинхронная задача.</returns>
    public static Task Set<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default) =>
        Set(cache, key, value, new DistributedCacheEntryOptions(), cancellationToken);

    /// <summary>
    /// Сохраняет значение в кэш с указанным ключом и дополнительными параметрами.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого значения.</typeparam>
    /// <param name="cache"><see cref="IDistributedCache"/></param>
    /// <param name="key">Ключ для сохранения значения.</param>
    /// <param name="value">Значение для сохранения в кэше.</param>
    /// <param name="options">Параметры записи в кэш.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Асинхронная задача.</returns>
    public static Task Set<T>(
        this IDistributedCache cache,
        string key,
        T value,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));

        return cache.SetAsync(key, bytes, options, cancellationToken);
    }

    /// <summary>
    /// Пытается получить значение из кэша по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения, которое требуется получить.</typeparam>
    /// <param name="cache"><see cref="IDistributedCache"/></param>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="value">Значение, полученное из кэша. Если значение не найдено, возвращается значение по умолчанию для типа T.</param>
    /// <returns>True, если значение было найдено и успешно десериализовано; в противном случае — False.</returns>
    public static bool TryGetValue<T>(this IDistributedCache cache, string key, [NotNullWhen(true)] out T? value)
    {
        byte[]? cacheValue = cache.Get(key);

        if (cacheValue is null)
        {
            value = default;
            return false;
        }

        value = JsonSerializer.Deserialize<T>(cacheValue, SerializerOptions);

        return value is not null;
    }
}

