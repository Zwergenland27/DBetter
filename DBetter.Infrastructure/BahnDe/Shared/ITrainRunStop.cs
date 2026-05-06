namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Classes that implement this interface contain base set of stop information
/// </summary>
public interface ITrainRunStop
{
    /// <summary>
    /// Fuzzy station id
    /// </summary>
    /// <example>A=1@O=Dresden Hbf@X=13732039@Y=51040562@U=80@L=8010085@i=U×008006050@</example>
    public string Id { get; set; }
    
    /// <summary>
    /// Name of the stop
    /// </summary>
    /// <example>Dresden Hbf</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Departure time information
    /// </summary>
    public Reisezeit? Abfahrt { get; set; }

    /// <summary>
    /// Arrival time information
    /// </summary>
    public Reisezeit? Ankunft { get; set; }
    
    /// <summary>
    /// Planned Platform
    /// </summary>
    /// <example>12</example>
    public string? Gleis { get; set; }
    
    /// <summary>
    /// Real Platform
    /// </summary>
    /// <example>12</example>
    public string? EzGleis { get; set; }
    
    /// <summary>
    /// Type of the platform
    /// </summary>
    public HaltTyp? HaltTyp { get; set; }
    
    /// <summary>
    /// Id for information about the station
    /// </summary>
    /// <example>1343</example>
    public string? BahnhofsInfoId { get; set; }
    
    /// <summary>
    /// Eva number of the station
    /// </summary>
    /// <example>8010085</example>
    public string ExtId { get; set; }
    
    /// <summary>
    /// Index of the station of the journey
    /// </summary>
    public int RouteIdx { get; set; }
}