namespace DBetter.Infrastructure.BahnDe.Routes.DTOs;

/// <summary>
/// Polyline containing coordinates
/// </summary>
public class PolyLineDescription
{
    /// <summary>
    /// List of Coordinates of the polyline
    /// </summary>
    public required List<Coordinate> Coordinates { get; set; }
}