using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.Requests.IncreaseTransferTime;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class ConnectionSnapshotFactory
{
    private readonly BahnDeUrlFactory _urlFactory;

    public ConnectionSnapshotFactory(SuggestionRequest request)
    {
        _urlFactory = new(request);
    }

    public ConnectionSnapshotFactory(IncreaseTransferTimeRequest request)
    {
        _urlFactory = new(request.OriginalRequest);
    }

    public ConnectionDto ExtractConnectionSnapshot(Verbindung verbindung)
    {
        List<SegmentDto> segments = [];

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK or VerkehrsmittelTyp.TRANSFER)
            {
                segments.Add(new WalkingSegmentDto
                {
                    Distance = abschnitt.Distanz!.Value,
                    WalkDuration = abschnitt.AbschnittsDauer
                });  
                
                continue;
            }
            
            if (segments.LastOrDefault() is TransportSegmentDto)
            {
                segments.Add(new TransferSegmentDto());
            }
            
            segments.Add(new TransportSegmentFactory(abschnitt).GetTransportSegment());
        }
        
        Offer? offer = null;
        if (verbindung.AngebotsPreis is not null)
        {
            var comfortClass = Klasse.GetComfortClassFromAlias(verbindung.AngebotsPreisKlasse!);
            var price = verbindung.AngebotsPreis.Betrag;
            var currency = verbindung.AngebotsPreis.Waehrung.ToCurrency();
            var partial = verbindung.HasTeilpreis;

            offer = new Offer(comfortClass, price, currency, partial);
        }

        return new ConnectionDto
        {
            ContextId = new ConnectionContextId(verbindung.CtxRecon),
            Demand = verbindung.GetDemand(),
            Segments = segments,
            Offer = offer,
            BookingLink = new BookingLink(_urlFactory.ForConnection(verbindung.CtxRecon))
        };
    }
}