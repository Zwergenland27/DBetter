using DBetter.Domain.Abstractions;
using DBetter.Domain.CoachLayouts.ValueObjects;
using DBetter.Domain.PlannedCoachLayouts.Coaches.ValueObjects;

namespace DBetter.Domain.PlannedCoachLayouts.Coaches;

public class PlannedCoach: Entity<PlannedCoachId>
{
    public CoachLayoutId LayoutId { get; set; }
    
    internal PlannedCoach(PlannedCoachId id, CoachLayoutId layoutId) : base(id)
    {
        LayoutId = layoutId;
    }
}