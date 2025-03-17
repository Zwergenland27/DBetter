using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class WalkTransferSection : Section
{
    public int Distance { get; private init; }
    
    public int WalkingMinutes { get; private init; }

    private WalkTransferSection(){}

    public WalkTransferSection(
        SectionId id,
        SectionIndex sectionIndex,
        int distance,
        int walkingMinutes) : base(id, sectionIndex)
    {
        Distance = distance;
        WalkingMinutes = walkingMinutes;
    }
}