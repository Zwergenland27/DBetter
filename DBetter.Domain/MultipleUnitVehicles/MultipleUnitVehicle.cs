using DBetter.Domain.Abstractions;
using DBetter.Domain.MultipleUnitVehicles.Coaches;
using DBetter.Domain.MultipleUnitVehicles.ValueObjects;
using DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;

namespace DBetter.Domain.MultipleUnitVehicles;

public class MultipleUnitVehicle: AggregateRoot<MultipleUnitVehicleId>
{
    private List<MultipleUnitCoach> _coaches;
    public MultipleUnitIdentifier Identifier { get; private set; }
 
    public MultipleUnitVehicleSeriesVariantId OfVariant { get; private set; }
    
    public ChristenedName? ChristenedName { get; private set; }
    
    public IReadOnlyList<MultipleUnitCoach> Coaches => _coaches.AsReadOnly();
    
    internal MultipleUnitVehicle(
        MultipleUnitVehicleId id,
        MultipleUnitIdentifier identifier,
        MultipleUnitVehicleSeriesVariantId ofVariant,
        ChristenedName? christenedName,
        List<MultipleUnitCoach> coaches) : base(id)
    {
        Identifier = identifier;
        OfVariant = ofVariant;
        ChristenedName = christenedName;
        _coaches = coaches;
    }
}