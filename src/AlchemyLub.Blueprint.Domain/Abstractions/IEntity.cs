namespace AlchemyLub.Blueprint.Domain.Abstractions;

/// <summary>
/// Базовая сущность домена
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Тип сущности
    /// </summary>
    public EntityType Type { get; }
}
