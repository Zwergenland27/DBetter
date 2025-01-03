namespace DBetter.Contracts.Journeys.Parameters;

public class RequestParameters
{
    public string Id { get; set; }
    public string? OwnerId { get; set; }
    public List<PassengerParameters> Passengers { get; set; }
    public DateTime Time { get; set; }
    public string TimeType { get; set; }
    public OptionsParameters Options { get; set; }
    public RouteParameters Route { get; set; }
}