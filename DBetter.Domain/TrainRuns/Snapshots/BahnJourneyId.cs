using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns.Snapshots;

public record BahnJourneyId(string Value)
{
    public TrainId TrainId => TrainId.Create(Parse(Value)["ZI"]).Value;
    public EvaNumber DestinationEvaNumber => EvaNumber.Create(Parse(Value)["LS"]).Value;
    
    public EvaNumber OriginEvaNumber => EvaNumber.Create(Parse(Value)["1S"]).Value;
    
    public OperatingDay OperatingDay => OperatingDay.Parse(Parse(Value)["DA"]);

    public TimeTableCompositeIdentifier TimeTableCompositeIdentifier => new (TrainId, TimeTablePeriod.FromOperatingDay(OperatingDay));
    
    public TrainRunCompositeIdentifier TrainRunCompositeIdentifier => new (TrainId, TimeTablePeriod.FromOperatingDay(OperatingDay), OperatingDay);
    
    private static Dictionary<string, string> Parse(string value)
    {
        var data = new Dictionary<string, string>();
        string[] parts = value.Split('#');

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