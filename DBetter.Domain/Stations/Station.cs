using DBetter.Domain.Abstractions;
using DBetter.Domain.Stations.Events;
using DBetter.Domain.Stations.Snapshots;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public class Station : AggregateRoot<StationId>
{
    public EvaNumber EvaNumber { get; private set; }
    
    public StationName Name { get; private set; }
    
    public Coordinates? Location { get; private set; }
    
    public StationInfoId? InfoId { get; private set; }
    
    public Ril100Identifier? Ril100 { get; private set; }

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
        Coordinates location) : base(id)
    {
        EvaNumber = evaNumber;
        Name = name;
        Location = location;
    }

    private Station() : base(null!){}

    public static Station CreateFromSnapshot(StationRouteSnapshot snapshot)
    {
        var station = new Station(StationId.CreateNew(), snapshot.EvaNumber, snapshot.Name, snapshot.InfoId);
        station.RaiseDomainEvent(new UnknownStationCreatedEvent(station.Id));
        return station;
    }
    
    public static Station CreateFromSnapshot(StationQuerySnapshot snapshot)
    {
        var station = new Station(StationId.CreateNew(), snapshot.EvaNumber, snapshot.Name, snapshot.Location);
        station.RaiseDomainEvent(new UnknownStationCreatedEvent(station.Id));
        return station;
    }

    public static Station CreateFromSnapshot(EvaNumber evaNumber, StationName name, StationInfoId? infoId)
    {
        var station = new Station(StationId.CreateNew(), evaNumber, name, infoId);
        station.RaiseDomainEvent(new UnknownStationCreatedEvent(station.Id));
        return station;
    }

    public void UpdateInformation(StationInformation stationInformation)
    {
        if (stationInformation.Position is not null)
        {
            Location = stationInformation.Position;
        }

        if (stationInformation.InfoId is not null)
        {
            InfoId = stationInformation.InfoId;
        }

        if (stationInformation.Ril100 is not null)
        {
            Ril100 = stationInformation.Ril100;
        }
    }
}
