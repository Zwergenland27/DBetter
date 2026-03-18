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
    public string Value { get; init; }
    public bool IsGerman => Value.StartsWith("80");

    private EvaNumber(string value)
    {
        Value = value;
    }

    public static CanFail<EvaNumber> Create(string value)
    {
        if (value.StartsWith("@L="))
        {
            value = value.Substring(3);
        }
        if (int.TryParse(value, out _))
        {
            return new EvaNumber(value);
        }

        return DomainErrors.Station.EvaNumber.NotNumeric(value);
    }

    public string AsFuzzy()
    {
        return $"@L={Value}";
    }
}