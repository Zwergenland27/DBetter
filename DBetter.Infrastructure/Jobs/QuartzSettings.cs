namespace DBetter.Infrastructure.Jobs;

public class QuartzSettings
{
    public const string SectionName = "Quartz";
    public string ConnectionString { get; init; } = null!;
}