namespace DBetter.Contracts.Shared.DTOs;

/// <summary>
/// Departure / arrival time
/// </summary>
public class TravelTimeDto
{
    /// <summary>
    /// Planned time
    /// </summary>
    public required string Planned  { get; set; }
    
    /// <summary>
    /// Real time
    /// </summary>
    public required string? Real { get; set; }
}