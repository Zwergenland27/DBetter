namespace DBetter.Contracts.Journeys.Parameters;

public class IncreaseTransferTimeRequestParameters
{
    public string Id { get; set; }
    public string? OwnerId { get; set; }
    
    public string ContextId { get; set; }
    public List<PassengerParameters> Passengers { get; set; }
    public OptionsParameters Options { get; set; }
    public RouteParameters Route { get; set; }
    public FixSectionStation Begin { get; set; }
    
    public FixSectionStation End { get; set; }
    
    public DateTime Time { get; set; }
    public string TimeType { get; set; }
}