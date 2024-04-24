namespace AlchemyLub.Blueprint.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    /// <inheritdoc />
    public DbSet<Entity> Entities => Set<Entity>();
}
