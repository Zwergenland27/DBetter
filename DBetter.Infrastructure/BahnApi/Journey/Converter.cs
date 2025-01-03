using DBetter.Contracts.Journeys.DTOs;
using DBetter.Infrastructure.BahnApi.Journey.Responses;

namespace DBetter.Infrastructure.BahnApi.Journey;

public static class Converter
{
    private static Dictionary<string, InformationDto?> _risMapping = new()
    {
        {"FT", new InformationDto{Priority = 1, Code = "Ris.Delay.FromPreviousJourney"}},
        {"text.realtime.connection.cancelled", new InformationDto{Priority = 2, Code = "Ris.Connection.Cancelled"}},
        {"text.realtime.journey.missed.connection", new InformationDto{Priority = 2, Code = "Ris.Connection.Missed"}},
        {"text.realtime.stop.entry.disabled", new InformationDto{Priority = 0, Code = "Ris.Stop.ExitOnly"}},
        {"text.realtime.stop.exit.disabled", new InformationDto{Priority = 0, Code = "Ris.Stop.EntryOnly"}},
        {"text.realtime.stop.cancelled", new InformationDto{Priority = 2, Code = "Ris.Stop.Cancelled"}},
        
    };
    public static InformationDto ToDto(this RisNotiz info)
    {
        if (_risMapping.TryGetValue(info.Key, out InformationDto? foundInfo))
        {
            return foundInfo!;
        }

        return new InformationDto
        {
            Code = info.Key,
            Priority = 2
        };
    }
}