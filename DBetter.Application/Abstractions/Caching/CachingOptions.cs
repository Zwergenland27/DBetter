namespace DBetter.Application.Abstractions.Caching;

public class CachingOptions
{
    public static CachingOptions Default { get; } = new()
    {
        Duration = TimeSpan.FromDays(1),
    };
    public TimeSpan Duration { get; init; }
}