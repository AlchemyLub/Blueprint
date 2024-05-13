namespace AlchemyLub.Blueprint.SharedKernel.Enums;

/// <summary>
/// Тип сущности
/// </summary>
public enum EntityType
{
    /// <summary>
    /// Неизвестный тип
    /// </summary>
    [OpenApiIgnoreEnum]
    Unknown = 0,

    /// <summary>
    /// Обычный тип
    /// </summary>
    Common = 1,

    /// <summary>
    /// Идентификатор
    /// </summary>
    Id = 2,
}
