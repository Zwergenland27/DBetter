using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.TrainRuns.Queries.Get.Results;

/// <summary>
/// Full trip of a public transport relation from start to destination station
/// </summary>
public class TrainRunResponse {

    /// <summary>
    /// Id of the train run
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Id of the corresponding train circulation
    /// </summary>
    public required string CirculationId { get; set; }

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
    /// Line number
    /// </summary>
    /// <example>RE 2</example>
    public required string? Line { get; set; }
    
    /// <summary>
    /// Stops of the train run
    /// </summary>
    public required List<StopResponse> Stops { get; set; }

    /// <summary>
    /// Service number of the train run
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
    
    /// <summary>
    /// Messages for passengers
    /// </summary>
    public required List<PassengerInformationResponse> PassengerInformation { get; set; }
}