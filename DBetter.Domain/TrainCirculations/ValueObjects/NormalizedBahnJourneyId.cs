using System.Text.RegularExpressions;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

/// <summary>
/// Journey Id of bahn.de
/// </summary>
/// <remarks>
/// DA Flag can be set to any date where the train run is valid -> skip between days is possible
/// </remarks>
public record NormalizedBahnJourneyId
{
    public string Value { get; private init; }

    private static readonly string operatingDatePlaceholder = "DA#_#";
    
    private NormalizedBahnJourneyId(string value)
    {
        Value = value;
    }

    public static NormalizedBahnJourneyId CreateNormalized(string value)
    {
        var normalized = RemoveDate(value);
        return new NormalizedBahnJourneyId(normalized);
    }

    public BahnJourneyId GenerateBahnJourneyId(OperatingDay operatingDay)
    {
        var asString = operatingDay.Date.ToString("ddMMyy");
        return new BahnJourneyId(Value.Replace(operatingDatePlaceholder, $"DA#{asString}#"));
    }

    private static string RemoveDate(string value)
    {
        return Regex.Replace(value, "DA#.*?#", operatingDatePlaceholder);
    }
}