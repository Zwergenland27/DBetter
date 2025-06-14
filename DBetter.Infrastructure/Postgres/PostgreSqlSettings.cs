namespace DBetter.Infrastructure.Postgres;

public class PostgreSqlSettings
{
    public const string SectionName = "PostgreSQL";
    public string ConnectionString { get; init; } = null!;
}