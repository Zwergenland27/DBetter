namespace DBetter.Contracts.Journeys.Parameters;

public class RouteParameters
{
    public StationDto? Origin { get; set; }
    public StationDto? Destination { get; set; }
    public List<ViaStationParameters> Via { get; set; }
    public List<RouteOptionParameters> RouteOptions { get; set; }
}