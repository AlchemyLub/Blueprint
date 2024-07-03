namespace AlchemyLub.Blueprint.Infrastructure.Database.Constants;

/// <summary>
/// Constants for PostgreSQL
/// </summary>
public static class PostgreSqlConstants
{
    public const string Name = "PostgreSql";

    public const int MaxRetryCount = 6;
    public static readonly TimeSpan MaxRetryDelay = TimeSpan.FromSeconds(10);
}
