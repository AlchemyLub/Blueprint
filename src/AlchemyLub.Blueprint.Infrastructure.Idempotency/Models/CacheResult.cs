namespace AlchemyLub.Blueprint.Infrastructure.Idempotency.Models;

/// <summary>
/// Результат попытки извлечения значения из кэша.
/// </summary>
/// <typeparam name="T">Тип значения, которое хранится в кэше.</typeparam>
/// <param name="Success">Указывает, было ли успешно извлечено значение из кэша</param>
/// <param name="Value">Извлечённое значение из кэша, или значение по умолчанию для типа <typeparamref name="T"/>, если извлечение не удалось</param>
public readonly record struct CacheResult<T>(bool Success, T? Value);
