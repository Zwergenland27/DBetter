namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

public class TeilstreckenStop
{
    /// <summary>
    /// Eva number of the section station
    /// </summary>
    /// <example>@L=8010085</example>
    public required string ExtId { get; set; }
    
    /// <summary>
    /// Timestamp of the section station
    /// </summary>
    public required string Zeitpunkt { get; set; }
}