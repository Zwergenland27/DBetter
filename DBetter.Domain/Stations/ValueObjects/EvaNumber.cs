using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Stations.ValueObjects;

/// <summary>
/// EVA number of the station
/// </summary>
/// <remarks>Also known as ibnr number</remarks>
/// <example>8010085</example>
public record EvaNumber
{
    public int Value { get; init; }
    private EvaNumber(int value)
    {
        Value = value;
    }

    public static CanFail<EvaNumber> Create(int value)
    {
        return new EvaNumber(value);
    }

    public static CanFail<EvaNumber> Create(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return new EvaNumber(result);
        }

        return DomainErrors.Station.EvaNumber.NotNumeric(value);
    }

    public string AsFuzzy()
    {
        return $"@L={Value}";
    }
}