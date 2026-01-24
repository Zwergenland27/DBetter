using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainRuns.ValueObjects;

/// <summary>
/// Journey Id of bahn.de
/// </summary>
/// <remarks>
/// DA Flag can be set to any date where the train run is valid -> skip between days is possible
/// </remarks>
public record BahnJourneyId(string Value)
{
    public EvaNumber DestinationEvaNumber => EvaNumber.Create(Parse()["LS"]).Value;
    
    public EvaNumber OriginEvaNumber => EvaNumber.Create(Parse()["1S"]).Value;

    private Dictionary<string, string> Parse()
    {
        var data = new Dictionary<string, string>();
        string[] parts = Value.Split('#');

        //Start at 1 because first part only contains version number without key
        //Increment by two because key and value are seperated by # as well
        for (int i = 1; i < parts.Length - 1; i += 2)
        {
            var key = parts[i];
            var valueOfKey = parts[i + 1];
            data[key] = valueOfKey;
        }

        return data;
    }
}