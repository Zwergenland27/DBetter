using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Stations.ValueObjects;

public record StationInfoId
{
    public int Value { get; }

    private StationInfoId(int value)
    {
        Value = value;
    }
    
    public static CanFail<StationInfoId> Create(int value)
    {
        return new StationInfoId(value);
    }

    public static CanFail<StationInfoId> Create(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return new StationInfoId(result);
        }

        return DomainErrors.Station.InfoId.NotNumeric(value);
    }
}