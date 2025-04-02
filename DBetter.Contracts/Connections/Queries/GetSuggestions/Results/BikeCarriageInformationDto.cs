namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Information about bike carriage
/// </summary>
public class BikeCarriageInformationDto
{
    /// <summary>
    /// Bike carriage status
    /// </summary>
    /// <example>ReservationRequired</example>
    public required string Status { get; set; }
    
    /// <summary>
    /// Stop index from which the information is valid
    /// </summary>
    public required int? FromStopIndex { get; set; }
    
    /// <summary>
    /// Stop index until which the information is valid
    /// </summary>
    public required int? ToStopIndex { get; set; }
}