namespace AlchemyLub.Blueprint.Infrastructure.Database.ContextConfigurations;

/// <summary>
/// Конфигурация для модели <see cref="Entity"/>
/// </summary>
public class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder
            .Property(t => t.Title)
            .HasMaxLength(100);

        builder
            .Property(t => t.Description)
            .HasMaxLength(500);

        builder
            .Property(t => t.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow);
    }
}
