using DBetter.Domain.Abstractions;
using DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.MultipleUnitVehicleSeriesVariant;

public class MultipleUnitVehicleSeriesVariant: AggregateRoot<MultipleUnitVehicleSeriesVariantId>
{
    public MultipleUnitVariantIdentifier Identifier { get; private set; }
    
    public PowerType PowerType { get; private set; }
    
    public SpeedLimit SpeedLimit { get; private set; }
    
    internal MultipleUnitVehicleSeriesVariant(
        MultipleUnitVehicleSeriesVariantId id,
        MultipleUnitVariantIdentifier identifier,
        PowerType powerType,
        SpeedLimit speedLimit) : base(id)
    {
        Identifier = identifier;
        PowerType = powerType;
        SpeedLimit = speedLimit;
    }
}