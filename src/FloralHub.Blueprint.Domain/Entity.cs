namespace FloralHub.Blueprint.Domain;

/// <summary>
/// Сущность домена
/// </summary>
public class Entity : IEntity
{
    /// <inheritdoc />
    public Guid Id { get; init; }

    /// <summary>
    /// Тип сущности
    /// </summary>
    public EntityType Type => EntityType.Common;
}
