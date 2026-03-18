using DBetter.Application;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public static class ParameterExtensions
{
    public static List<StationId> GetAllStops(this ConnectionRequest request)
    {
        List<StationId> stationIds =
        [
            request.PlannedRoute.OriginStationId,
            request.PlannedRoute.DestinationStationId
        ];

        if(request.PlannedRoute.FirstStopover is not null)
        {
            stationIds.Add(request.PlannedRoute.FirstStopover.StationId);
        }

        if(request.PlannedRoute.SecondStopover is not null)
        {
            stationIds.Add(request.PlannedRoute.SecondStopover.StationId);
        }

        return stationIds;
    }

    public static TeilstreckeAnfrage ToRequest(
        this ReiseAnfrage bahnRequest,
        string contextId,
        EvaNumber fixedStartEvaNumber,
        DateTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        DateTime fixedEndTime)
    {
        var fixedSectionBegin = new TeilstreckenStop
        {
            ExtId = fixedStartEvaNumber.Value,
            Zeitpunkt = fixedStartTime.ToBahnTime()
        };
        
        var fixedSectionEnd = new TeilstreckenStop
        {
            ExtId = fixedEndEvaNumber.Value,
            Zeitpunkt = fixedEndTime.ToBahnTime()
        };
        
        return new TeilstreckeAnfrage
        {
            Klasse = bahnRequest.Klasse,
            AnkunftSuche = bahnRequest.AnkunftSuche,
            Produktgattungen = bahnRequest.Produktgattungen,
            Reisende = bahnRequest.Reisende,
            MaxUmstiege = bahnRequest.MaxUmstiege,
            SchnelleVerbindungen = bahnRequest.SchnelleVerbindungen,
            SitzplatzOnly = bahnRequest.SitzplatzOnly,
            BikeCarriage = bahnRequest.BikeCarriage,
            NurDeutschlandTicketVerbindungen = bahnRequest.NurDeutschlandTicketVerbindungen,
            Zwischenhalte = bahnRequest.Zwischenhalte,
            FixedTeilstrecke = new FixedTeilstrecke
            {
                Begin = fixedSectionBegin,
                End = fixedSectionEnd
            },
            OriginalCtxRecon = contextId
        };
    }
}