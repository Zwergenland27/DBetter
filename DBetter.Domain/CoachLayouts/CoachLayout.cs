using DBetter.Domain.Abstractions;
using DBetter.Domain.CoachLayouts.ValueObjects;

namespace DBetter.Domain.CoachLayouts;

public class CoachLayout: AggregateRoot<CoachLayoutId>
{
    public CoachLayoutIdentifier Identifier { get; private set; }
    
    public ConstructionType ConstructionType { get; private set; }
    
    public Amenities Amenities { get; private set; }
    
    internal CoachLayout(
        CoachLayoutId id,
        CoachLayoutIdentifier identifier,
        ConstructionType constructionType,
        Amenities amenities) : base(id)
    {
        Identifier = identifier;
        ConstructionType = constructionType;
        Amenities = amenities;
    }
}