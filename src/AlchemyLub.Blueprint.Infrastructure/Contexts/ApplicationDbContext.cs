namespace AlchemyLub.Blueprint.Infrastructure.Contexts;

/// <summary>
/// Контекст базы данных сервиса
/// </summary>
/// <param name="options"><see cref="DbContextOptions{ApplicationDbContext}"/></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция сущностей домена
    /// </summary>
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

        NpgsqlConnectionStringBuilder connectionStringBuilder = new()
        {
            Host = "Server",
            Port = 5432,
            Database = "Database",
            Username = "UserId",
            Password = "Password",
            CommandTimeout = 20,
            Timeout = 5,
            Pooling = true,
            LogParameters = true,
            KeepAlive = 10,
        };

        optionsBuilder.UseNpgsql(connectionStringBuilder.ToString());
    }
}
