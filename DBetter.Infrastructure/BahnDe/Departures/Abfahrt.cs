namespace DBetter.Infrastructure.BahnDe.Departures;

public class Abfahrt
{
    public required string JourneyId { get; set; }
    
    public required Verkehrsmittel Verkehrmittel { get; set; }
}