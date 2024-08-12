namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Handlers.Abstractions;

public interface IIdempotencyService
{
    Task<T?> GetIdempotencyResult<T>(
        string idempotencyKey,
        Func<string, CancellationToken, T> func,
        CancellationToken cancellationToken = default);

    Task SetIdempotencyResult<T>(string idempotencyKey, T value, CancellationToken cancellationToken = default);
}
