namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Handlers;

public class IdempotencyService(IDistributedCache distributedCache, IMemoryCache memoryCache) : IIdempotencyService
{
    /// <inheritdoc />
    public async Task<T?> GetIdempotencyResult<T>(
        string idempotencyKey,
        Func<string, CancellationToken, T> func,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(idempotencyKey);

        if (memoryCache.TryGetValue(idempotencyKey, out string? cachedResponse) && cachedResponse is not null)
        {
            return JsonSerializer.Deserialize<T>(cachedResponse);
        }

        cachedResponse = await distributedCache.GetStringAsync(idempotencyKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            memoryCache.Set(idempotencyKey, cachedResponse);

            return JsonSerializer.Deserialize<T>(cachedResponse);
        }

        return func(idempotencyKey, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SetIdempotencyResult<T>(string idempotencyKey, T value, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(idempotencyKey);

        await Task.CompletedTask;
    }
}
