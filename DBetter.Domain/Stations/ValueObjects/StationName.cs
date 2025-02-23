using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Stations.ValueObjects;

/// <summary>
/// Name of a station
/// </summary>
/// <remarks>
/// Since the naming provided by Bahn.de is not consistent, the name will be normalized
/// </remarks>
/// <example>Dresden Hbf</example>
public record StationName
{
    public string Value { get; init; }

    private StationName(string value)
    {
        Value = value;
    }

    public static CanFail<StationName> Create(string stationName)
    {
        if (IsMetaStation(stationName)) return DomainErrors.Station.Name.MetaStation(stationName);
        stationName = Normalize(stationName);
        return new StationName(stationName);
    }

    private static bool IsMetaStation(string value)
    {
        return value.Where(char.IsLetter).All(char.IsUpper);
    }

    private static string Normalize(string value)
    {
        return value
            .Replace("(", " (")
            .Replace(")", ") ")
            .Replace("(b ", "(")
            .Trim();
    }
}