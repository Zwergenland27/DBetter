using DBetter.Domain.Abstractions;
using DBetter.Domain.VehicleSeries.ValueObjects;

namespace DBetter.Domain.VehicleSeries;

public class VehicleSeries: AggregateRoot<VehicleSeriesId>
{
    public VehicleSeriesIdentifier Identifier { get; private set; }
    
    public TractionUnit MaxTractionUnits { get; private set; }
    
    internal VehicleSeries(
        VehicleSeriesId id,
        VehicleSeriesIdentifier identifier,
        TractionUnit maxTractionUnit) : base(id)
    {
        Identifier = identifier;
        MaxTractionUnits = maxTractionUnit;
    }

    public static VehicleSeries Create(string rawIdentifier)
    {
        return new VehicleSeries(VehicleSeriesId.CreateNew(), new VehicleSeriesIdentifier(rawIdentifier), new TractionUnit(3));
    }
}