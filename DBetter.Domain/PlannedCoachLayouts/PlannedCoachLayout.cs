using DBetter.Domain.Abstractions;
using DBetter.Domain.PlannedCoachLayouts.Coaches;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;

namespace DBetter.Domain.PlannedCoachLayouts;

public class PlannedCoachLayout: AggregateRoot<PlannedCoachLayoutId>
{
    private List<PlannedCoach> Coaches { get; set; }
    
    protected PlannedCoachLayout(PlannedCoachLayoutId id, List<PlannedCoach> coaches) : base(id)
    {
        Coaches = coaches;
    }
}
