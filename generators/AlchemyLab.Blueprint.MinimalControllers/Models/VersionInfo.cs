namespace AlchemyLab.Blueprint.MinimalControllers.Generator.Models;

/// <summary>
/// Информация о версии
/// </summary>
/// <param name="Version">Значение версии</param>
/// <param name="Status">Статус версии. Например, значения могут включать "alpha", "beta", "rc" и т.д.</param>
/// <param name="IsDeprecated">Флаг устаревания версии</param>
public record VersionInfo(string Version, string? Status = null, bool IsDeprecated = false);
