namespace DBetter.Contracts.Shared.DTOs;

/// <summary>
/// Information about catering
/// </summary>
public class CateringInformationDto
{
    /// <summary>
    /// Catering type
    /// </summary>
    /// <example>Restaurant</example>
    public required string Type { get; set; }
    
    /// <summary>
    /// Stop index from which the information is valid
    /// </summary>
    public required int? FromStopIndex { get; set; }
    
    /// <summary>
    /// Stop index until which the information is valid
    /// </summary>
    public required int? ToStopIndex { get; set; }
}