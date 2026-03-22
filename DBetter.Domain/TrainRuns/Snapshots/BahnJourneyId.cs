using System.ComponentModel.DataAnnotations.Schema;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns.Snapshots;

public record BahnJourneyId
{
    public string Value { get; private init; }

    private BahnJourneyId(string value)
    {
        Value = value;
    }
    public EvaNumber DestinationEvaNumber => EvaNumber.Create(Parse(Value)["LS"]).Value;
    
    public EvaNumber OriginEvaNumber => EvaNumber.Create(Parse(Value)["1S"]).Value;

    public OperatingDay OperatingDay => GetOperatingDay(Value);

    public TrainCirculationIdentifier TrainCirculationIdentifier => GetTrainCirculationIdentifier(Value);
    
    public TrainRunIdentifier TrainRunIdentifier => new (TrainCirculationIdentifier, OperatingDay);

    private static TrainCirculationIdentifier GetTrainCirculationIdentifier(string value)
    {
        var values = Parse(value);
        
        var departureTime = HafasTime.Create(values["1T"]);
        var arrivalTime = HafasTime.Create(values["LT"]);
        var travelDuration = TravelDuration.Create(departureTime, arrivalTime);
        
        var originStationEva = EvaNumber.Create(values["1S"]).Value;
        var destinationStationEva = EvaNumber.Create(values["LS"]).Value;
        
        return new TrainCirculationIdentifier(originStationEva, departureTime.Time, destinationStationEva, travelDuration);
    }

    private static OperatingDay GetOperatingDay(string value)
    {
        var values = Parse(value);
        
        var operatingDay = OperatingDay.Parse(values["DA"]);
        var departureTime = HafasTime.Create(values["1T"]);
        
        return operatingDay.CorrectTimeOffset(departureTime);
    }
    
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

    public static BahnJourneyId Create(string value)
    {
        var normalizedValue = value.Replace("#RT#3#", "#RT#1#");
        return new BahnJourneyId(normalizedValue);
    }
}