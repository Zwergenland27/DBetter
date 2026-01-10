using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class ConnectionSnapshotFactory(SuggestionRequest request, Fahrplan fahrplan)
{
    private BahnDeUrlFactory _urlFactory = new(request);

    public SuggestionResponse ExtractSnapshot()
    {
        var earlierToken = fahrplan.VerbindungReference.Earlier;
        var laterToken = fahrplan.VerbindungReference.Later;
        
        return new SuggestionResponse
        {
            Connections = fahrplan.Verbindungen.Select(ExtractConnectionSnapshot).ToList(),
            EarlierRef = earlierToken is null ? null : PaginationReference.Create(earlierToken),
            LaterRef = laterToken is null ? null : PaginationReference.Create(laterToken)
        };
    }

    private ConnectionSnapshot ExtractConnectionSnapshot(Verbindung verbindung)
    {
        List<SegmentSnapshot> segments = [];

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK or VerkehrsmittelTyp.TRANSFER)
            {
                segments.Add(new WalkingSegmentSnapshot
                {
                    Distance = abschnitt.Distanz!.Value,
                    WalkDuration = abschnitt.AbschnittsDauer
                });  
                
                continue;
            }
            
            if (segments.LastOrDefault() is TransportSegmentSnapshot)
            {
                segments.Add(new TransferSegmentSnapshot());
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

        return new ConnectionSnapshot
        {
            ContextId = new ConnectionContextId(verbindung.CtxRecon),
            Demand = verbindung.GetDemand(),
            Segments = segments,
            Offer = offer,
            BookingLink = new BookingLink(_urlFactory.ForConnection(verbindung.CtxRecon))
        };
    }
}