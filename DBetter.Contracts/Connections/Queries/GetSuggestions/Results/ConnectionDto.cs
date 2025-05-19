using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// A connection from origin to destination station
/// </summary>
public class ConnectionDto
{
    /// <summary>
    /// Id of the connection
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandDto Demand { get; set; }
    
    /// <summary>
    /// Sections of the connection
    /// </summary>
    public required List<SegmentDto> Segments { get; set; }
    
    /// <summary>
    /// Offer for the section, if available
    /// </summary>
    public required OfferDto? Offer { get; set; }
    
    /// <summary>
    /// Url to bahn.de to view and buy the connection
    /// </summary>
    public required string BahnDeUrl { get; set; }
}