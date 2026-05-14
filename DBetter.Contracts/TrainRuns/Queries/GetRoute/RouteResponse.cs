namespace DBetter.Contracts.TrainRuns.Queries.GetRoute;

public class RouteResponse
{
    public required int? ServiceNumber { get; init; }
    
    public required List<RouteStopResponse> Stops { get; init; }
}