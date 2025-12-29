using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Routes.DTOs;

public class JourneyIdParser
{
    private readonly string _journeyId;
    private readonly Dictionary<string, string> _data = [];
    
    public EvaNumber DestinationEvaNumber => EvaNumber.Create(_data["LS"]).Value;
    
    public EvaNumber OriginEvaNumber => EvaNumber.Create(_data["S"]).Value;
    
    public JourneyIdParser(string journeyId)
    {
        _journeyId = journeyId;
        string[] parts = journeyId.Split('#');

        //Start at 1 because first part only contains version number without key
        //Increment by two because key and value are seperated by # as well
        for (int i = 1; i < parts.Length - 1; i += 2)
        {
            var key = parts[i];
            var value = parts[i + 1];
            _data[key] = value;
        }
    }
    
    public BahnJourneyId ToDomain()
    {
        return new BahnJourneyId(_journeyId);
    }
}