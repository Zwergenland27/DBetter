using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Stations.ValueObjects;

public record StationInfoId
{
    public string Value { get; }

    private StationInfoId(string value)
    {
        Value = value;
    }

    public static CanFail<StationInfoId> Create(string value)
    {
        if (int.TryParse(value, out _))
        {
            return new StationInfoId(value);
        }

        return DomainErrors.Station.InfoId.NotNumeric(value);
    }
}