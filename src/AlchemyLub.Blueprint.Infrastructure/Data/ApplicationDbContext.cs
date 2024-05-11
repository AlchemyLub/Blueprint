namespace AlchemyLub.Blueprint.Infrastructure.Data;

/// <summary>
/// Контекст базы данных сервиса
/// </summary>
/// <param name="options"><see cref="DbContextOptions{ApplicationDbContext}"/></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    /// <inheritdoc />
    public DbSet<Entity> Entities => Set<Entity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EntityConfiguration());
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // TODO: Дописать инициализацию строки подключения
        NpgsqlConnectionStringBuilder connectionStringBuilder = new()
        {
            Host = "<Server>",
            Port = 5432
        };

        optionsBuilder.UseNpgsql(
            connectionStringBuilder.ToString(),
            options =>
            {
                return;
            });
        }
}
