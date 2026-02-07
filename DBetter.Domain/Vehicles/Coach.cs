using DBetter.Domain.Abstractions;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public class Coach : Entity<CoachId>
{
    public string? ConstructionType { get; private init; }
    
    public string? CoachType { get; private init; }

    private Coach(
        CoachId id,
        string? constructionType,
        string? coachType) : base(id)
    {
        ConstructionType = constructionType;
        CoachType = coachType;
    }

    internal static Coach CreateByConstructionType(CoachId id, string constructionType)
    {
        return new Coach(id, constructionType, null);
    }
    
    internal static Coach CreateByCoachType(CoachId id, string coachType)
    {
        return new Coach(id, null, coachType);
    }
}