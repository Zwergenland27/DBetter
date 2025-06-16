namespace DBetter.Infrastructure.Monitoring;

public class OtelSettings
{
    public const string SectionName = "Otel";
    public string Endpoint { get; init; } = null!;
}