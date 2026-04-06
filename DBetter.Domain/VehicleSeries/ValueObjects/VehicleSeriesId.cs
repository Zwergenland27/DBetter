using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.VehicleSeries.ValueObjects;

public record VehicleSeriesId(Guid Value)
{
    public static VehicleSeriesId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<VehicleSeriesId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new VehicleSeriesId(guid);
        }

        return DomainErrors.VehicleSeries.Id.Invalid(value);
    }
}