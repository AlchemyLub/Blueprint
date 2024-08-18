using AlchemyLub.Blueprint.Infrastructure.Idempotency.Models;

namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Extensions;

public static class MemoryCacheExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Получает значение из кэша по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения, которое требуется получить.</typeparam>
    /// <param name="cache"><see cref="IMemoryCache"/></param>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Значение, полученное из кэша, или значение по умолчанию для типа <typeparamref name="T"/>, если ключ не найден.</returns>
    public static Task<T?> Get<T>(this IMemoryCache cache, string key, CancellationToken cancellationToken = default) =>
        Task.Run(() =>
        {
            if (cache.TryGetValue(key, out string? json) && json != null)
            {
                return JsonSerializer.Deserialize<T>(json, SerializerOptions);
            }

            return default;
        }, cancellationToken);

    /// <summary>
    /// Пытается получить значение из кэша по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения, которое требуется получить.</typeparam>
    /// <param name="cache"><see cref="IMemoryCache"/></param>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>True, если значение было найдено и успешно десериализовано; в противном случае — False.</returns>
    public static ValueTask<CacheResult<T>> TryGetValue<T>(this IMemoryCache cache, string key, CancellationToken cancellationToken = default)
    {
        var one = Task.Run(() =>
        {
            if (cache.TryGetValue(key, out string? json) && json != null)
            {
                T? value = JsonSerializer.Deserialize<T>(json, SerializerOptions);

                return new CacheResult<T>(value != null, value);
            }

            return new CacheResult<T>(false, default);
        });

        if (cache.TryGetValue(key, out string? json) && json != null)
        {
            value = JsonSerializer.Deserialize<T>(json, SerializerOptions);

            return value is not null;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Пытается получить значение из кэша по указанному ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения, которое требуется получить.</typeparam>
    /// <param name="cache"><see cref="IMemoryCache"/></param>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="value">Значение, полученное из кэша. Если значение не найдено, возвращается значение по умолчанию для типа <typeparamref name="T"/></param>
    /// <returns>True, если значение было найдено и успешно десериализовано; в противном случае — False.</returns>
    public static bool TryGetSync<T>(this IMemoryCache cache, string key, [NotNullWhen(true)] out T? value)
    {
        if (cache.TryGetValue(key, out string? json) && json != null)
        {
            value = JsonSerializer.Deserialize<T>(json, SerializerOptions);

            return value is not null;
        }

        value = default;
        return false;
    }

    public static Task Set<T>(this IMemoryCache cache, string key, T value, MemoryCacheEntryOptions? options = null)
    {
        string json = JsonSerializer.Serialize(value, SerializerOptions);
        return Task.Run(() => cache.Set(key, json, options));
    }

    /// <summary>
    /// Сохраняет значение в память с указанным ключом.
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого значения.</typeparam>
    /// <param name="cache"><see cref="IMemoryCache"/></param>
    /// <param name="key">Ключ для сохранения значения.</param>
    /// <param name="value">Значение для сохранения в кэше.</param>
    /// <param name="options">Параметры записи в кэш (опционально).</param>
    public static void SetSync<T>(this IMemoryCache cache, string key, T value, MemoryCacheEntryOptions? options = null)
    {
        string json = JsonSerializer.Serialize(value, SerializerOptions);
        if (options != null)
        {
            cache.Set(key, json, options);
        }
        else
        {
            cache.Set(key, json);
        }
    }
}
