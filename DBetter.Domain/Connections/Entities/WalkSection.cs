using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class WalkSection : Section
{
    public int Distance { get; private init; }
    
    public int WalkingMinutes { get; private init; }

    private WalkSection() {}

    public WalkSection(
        SectionId id,
        int distance,
        int walkingMinutes) : base(id)
    {
        Distance = distance;
        WalkingMinutes = walkingMinutes;
    }
}