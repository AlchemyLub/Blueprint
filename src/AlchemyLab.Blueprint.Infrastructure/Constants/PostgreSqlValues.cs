namespace AlchemyLab.Blueprint.Infrastructure.Database.Constants;

/// <summary>
/// Values for PostgreSQL
/// </summary>
public static class PostgreSqlValues
{
    public const string Name = "PostgreSql";

    public const int MaxRetryCount = 6;
    public static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(10);
}
