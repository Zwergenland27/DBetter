using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Routes.Queries.Get.Results;

/// <summary>
/// Full trip of a public transport relation from start to destination station
/// </summary>
public class RouteResponse {

    /// <summary>
    /// Id of the route
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Operator of the service
    /// </summary>
    /// <example>DB Regio Südost</example>
    public required string? Operator { get; set; } 

    /// <summary>
    /// The transport category
    /// </summary>
    /// <example>HighSpeedTrains</example>
    public required string TransportCategory { get; set; }
    
    /// <summary>
    /// Detailed Class of the transportation product
    /// </summary>
    /// <example>RB</example>
    public required string ProductClass { get; set; }
    
    /// <summary>
    /// Line number
    /// </summary>
    /// <example>RE 2</example>
    public required string? Line { get; set; }
    
    /// <summary>
    /// Stops of the route
    /// </summary>
    public required List<StopResponse> Stops { get; set; }

    /// <summary>
    /// Service number of the route
    /// </summary>
    /// <example>1645</example>
    public required int? ServiceNumber { get; set; }
    
    /// <summary>
    /// Information about bike carriage
    /// </summary>
    public required BikeCarriageInformationDto BikeCarriage { get; set; }
    
    /// <summary>
    /// Information about catering in the vehicle
    /// </summary>
    public required CateringInformationDto Catering { get; set; }
}