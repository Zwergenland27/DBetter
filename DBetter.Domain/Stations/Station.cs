using DBetter.Domain.Abstractions;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public class Station : AggregateRoot<StationId>
{
    public EvaNumber EvaNumber { get; private set; }
    
    public StationName Name { get; private set; }
    
    public Coordinates? Position { get; private set; }
    
    public StationInfoId? InfoId { get; private set; }
    
    public Ril100? Ril100 { get; private set; }
    
    public DateTime? LastScrapedAt { get; private set; }

    public Station(
        StationId id,
        EvaNumber evaNumber,
        StationName name,
        Coordinates? position,
        StationInfoId? infoId) : base(id)
    {
        EvaNumber = evaNumber;
        Name = name;
        Position = position;
        InfoId = infoId;
    }

    private Station(StationId id) : base(id)
    {
        
    }

    public void UpdateScrapedInformation(
        Coordinates? position,
        StationInfoId? infoId,
        Ril100? ril100)
    {
        Position = position;
        InfoId = infoId;
        Ril100 = ril100;
        LastScrapedAt = DateTime.UtcNow;
    }
}