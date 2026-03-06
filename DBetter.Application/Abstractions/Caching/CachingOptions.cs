namespace DBetter.Application.Abstractions.Caching;

public class CachingOptions
{
    public static CachingOptions Default { get; } = new()
    {
        Duration = TimeSpan.MaxValue
    };
    public TimeSpan Duration { get; init; }
}