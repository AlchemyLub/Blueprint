namespace AlchemyLub.Blueprint.Infrastructure.Contexts;

/// <summary>
/// Application database context
/// </summary>
/// <param name="options">The options for the database context.</param>
[ExcludeFromCodeCoverage]
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Entities database set.
    /// </summary>
    public DbSet<Entity> Entities => Set<Entity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EntityConfiguration());
    }
}
