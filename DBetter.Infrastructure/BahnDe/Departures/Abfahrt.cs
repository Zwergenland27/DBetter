using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Departures;

public class Abfahrt
{
    public required string JourneyId { get; set; }
    
    public required Verkehrsmittel Verkehrmittel { get; set; }
    
    public List<PriorisierteMeldung>? Meldungen { get; set; }
}