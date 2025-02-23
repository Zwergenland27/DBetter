using DBetter.Domain.Abstractions;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public class Station : AggregateRoot<StationId>
{
    public EvaNumber EvaNumber { get; private set; }
    
    public StationName Name { get; private set; }
    
    public Coordinates? Position { get; private set; }

    public Station(
        StationId id,
        EvaNumber evaNumber,
        StationName name,
        Coordinates? position) : base(id)
    {
        EvaNumber = evaNumber;
        Name = name;
        Position = position;
    }

    private Station(StationId id) : base(id)
    {
        
    }

    public void UpdatePosition(Coordinates position)
    {
        Position = position;
    }
}