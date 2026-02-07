using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Vehicles.ValueObjects;

public record VehicleId(Guid Value)
{
    public static VehicleId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<VehicleId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new VehicleId(guid);
        }

        return DomainErrors.Vehicle.Id.Invalid(value);
    }
}