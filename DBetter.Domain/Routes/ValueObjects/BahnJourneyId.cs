using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Routes.ValueObjects;

/// <summary>
/// Journey Id of bahn.de
/// </summary>
/// <remarks>
/// DA Flag can be set to any date where the train run is valid -> skip between days is possible
/// </remarks>
public record BahnJourneyId
{
    public string Value { get; private init; }
    
    private readonly Dictionary<string, string> _data = [];
    
    public EvaNumber DestinationEvaNumber => EvaNumber.Create(_data["LS"]).Value;
    
    public EvaNumber OriginEvaNumber => EvaNumber.Create(_data["1S"]).Value;
    
    public BahnJourneyId(string value)
    {
        Value = value;
        string[] parts = value.Split('#');

        //Start at 1 because first part only contains version number without key
        //Increment by two because key and value are seperated by # as well
        for (int i = 1; i < parts.Length - 1; i += 2)
        {
            var key = parts[i];
            var valueOfKey = parts[i + 1];
            _data[key] = valueOfKey;
        }
    }
}