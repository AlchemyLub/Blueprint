namespace AlchemyLub.Blueprint.Domain.Enums;

/// <summary>
/// Тип сущности
/// </summary>
public enum EntityType
{
    // TODO: Нужно игнорировать такие вещи в swagger
    /// <summary>
    /// Неизвестный тип
    /// </summary>
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
