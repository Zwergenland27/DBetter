namespace DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

public class Coordinate
{
    /// <summary>
    /// Longitude of a coordinate
    /// </summary>
    /// <example>14.234688</example>
    public required float Lng { get; set; }
    
    /// <summary>
    /// Longitude of a coordinate
    /// </summary>
    /// <example>50.87588</example>
    public required float Lat { get; set; }
}