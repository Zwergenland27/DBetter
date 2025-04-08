
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Routes.Queries.Get;

/// <summary>
/// Full trip of a public transport relation from start to destination station
/// </summary>
public class RouteDto {

    /// <summary>
    /// Id of the route
    /// </summary>
    public required string RouteId { get; set; }

    /// <summary>
    /// Operator of the service
    /// </summary>
    /// <example>DB Regio Südost</example>
    public required string? Operator { get; set; } 

    /// <summary>
    /// The transport product
    /// </summary>
    /// <example>RegionalExpress</example>
    public required string Product { get; set; }
    
    /// <summary>
    /// Stops of the route
    /// </summary>
    public required List<StopDto> Stops { get; set; }

    /// <summary>
    /// Line number of the route
    /// </summary>
    /// <example>2</example>
    public required string? LineNumber { get; set; }

    /// <summary>
    /// Service number of the route
    /// </summary>
    /// <example>1645</example>
    public required string? ServiceNumber { get; set; }
    
    /// <summary>
    /// Information about bike carriage
    /// </summary>
    public required BikeCarriageInformationDto BikeCarriage { get; set; }
    
    /// <summary>
    /// Information about catering in the vehicle
    /// </summary>
    public required CateringInformationDto Catering { get; set; }
}