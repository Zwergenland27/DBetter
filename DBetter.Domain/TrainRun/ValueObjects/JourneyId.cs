namespace DBetter.Domain.TrainRun.ValueObjects;

/// <summary>
/// Journey Id of bahn.de
/// </summary>
/// <remarks>
/// DA Flag can be set to any date where the train run is valid -> skip between days is possible
/// </remarks>
public record JourneyId(string Value)
{
    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> data = [];
        string[] parts = Value.Split('#');

        //Start at 1 because first part only contains version number without key
        //Increment by two because key and value are seperated by # as well
        for (int i = 1; i < parts.Length - 1; i += 2)
        {
            var key = parts[i];
            var value = parts[i + 1];
            data[key] = value;
        }

        return data;
    }
}