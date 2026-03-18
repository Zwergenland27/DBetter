using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations.Events;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public class Station : AggregateRoot<StationId>
{
    public EvaNumber EvaNumber { get; private set; }
    
    public StationName Name { get; private set; }
    
    public Coordinates? Location { get; private set; }
    
    public StationInfoId? InfoId { get; private set; }
    
    public Ril100Identifier? Ril100 { get; private set; }
    
    public MeansOfTransport AvailableMeansOfTransport { get; private set; }

    private Station(
        StationId id,
        EvaNumber evaNumber,
        StationName name,
        StationInfoId? infoId) : base(id)
    {
        EvaNumber = evaNumber;
        Name = name;
        InfoId = infoId;
    }
    
    private Station(
        StationId id,
        EvaNumber evaNumber,
        StationName name,
        Coordinates location,
        MeansOfTransport availableMeansOfTransport) : base(id)
    {
        EvaNumber = evaNumber;
        Name = name;
        Location = location;
        AvailableMeansOfTransport = availableMeansOfTransport;
    }

    private Station() : base(null!){}
    
    public static Station Create(EvaNumber evaNumber, StationName name, Coordinates location, MeansOfTransport availableMeansOfTransport)
    {
        var station = new Station(StationId.CreateNew(), evaNumber, name, location, availableMeansOfTransport);
        station.RaiseDomainEvent(new UnknownStationCreatedEvent(station.Id));
        return station;
    }

    public static Station Create(EvaNumber evaNumber, StationName name, StationInfoId? infoId)
    {
        var station = new Station(StationId.CreateNew(), evaNumber, name, infoId);
        station.RaiseDomainEvent(new UnknownStationCreatedEvent(station.Id));
        return station;
    }

    public void Update(Coordinates location)
    {
        Location = location;
    }

    public void Update(StationInfoId infoId)
    {
        InfoId = infoId;
    }

    public void Update(MeansOfTransport availableMeansOfTransport)
    {
        AvailableMeansOfTransport = availableMeansOfTransport;
    }

    public void Update(Ril100Identifier ri100Identifier)
    {
        Ril100 = ri100Identifier;
    }

    public void ScrapeDepartures(DateOnly forDay)
    {
        RaiseDomainEvent(new StationDeparturesScrapingScheduledEvent(Id, forDay));
    }
}
