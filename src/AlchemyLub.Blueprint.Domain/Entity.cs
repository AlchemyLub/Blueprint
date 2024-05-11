namespace AlchemyLub.Blueprint.Domain;

/// <summary>
/// Сущность домена
/// </summary>
public class Entity(Guid id) : IEntity
{
    /// <inheritdoc />
    public Guid Id => id;

    /// <summary>
    /// Заголовок
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Дата создания сущности
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <inheritdoc />
    public EntityType Type => EntityType.Common;
}
