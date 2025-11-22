namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

public class FixedTeilstrecke
{
    /// <summary>
    /// Start of the fixed section
    /// </summary>
    public required TeilstreckenStop Begin { get; set; }
    
    /// <summary>
    /// End of the fixed section
    /// </summary>
    public required TeilstreckenStop End { get; set; }
}