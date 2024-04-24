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

    /// <inheritdoc />
    public EntityType Type => EntityType.Common;
}
