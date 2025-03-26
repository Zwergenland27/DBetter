using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.BahnDe.TrainRuns.Entities;

namespace DBetter.Infrastructure.BahnDe.Connections;

public static class DTOExtensions
{
    public static Connection ToDomain(
        this Verbindung verbindung,
        ConnectionRequestId requestId,
        Dictionary<string, TrainRunEntity> trainRunMapping,
        out List<TrainRunEntity> newTrainRuns)
    {
        Offer? offer = null;
        if (verbindung.AngebotsPreis is not null)
        {
            offer = new Offer(
                verbindung.AngebotsPreisKlasse!.Value.ToDomainClass(),
                verbindung.AngebotsPreis.Betrag,
                verbindung.AngebotsPreis.Waehrung.ToDomainCurrency(),
                verbindung.HasTeilpreis
            );
        }

        List<Section> sections = [];
        newTrainRuns = [];

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK)
            {
                sections.Add(new WalkSection(
                    SectionId.CreateNew(),
                    abschnitt.Distanz!.Value,
                    abschnitt.AbschnittsDauer));
                continue;
            }
            
            sections.Add(abschnitt.ToDomain(trainRunMapping, out var newTrainRun));

            if (newTrainRun is not null)
            {
                newTrainRuns.Add(newTrainRun);
            }
        }
        
        return new Connection(
            ConnectionId.CreateNew(),
            requestId,
            new TripId(verbindung.TripId),
            offer,
            verbindung.GetDomainMessages(),
            verbindung.Auslastungsmeldungen.GetDomainDemand(),
            verbindung.FahrradmitnahmeMoeglich.GetBikeCarriageInformation(),
            sections);
    }

    private static ComfortClass ToDomainClass(this Klasse klasse)
    {
        return klasse switch
        {
            Klasse.KLASSE_1 => ComfortClass.First,
            Klasse.KLASSE_2 => ComfortClass.Second,
            _ => throw new BahnDeException("ConnectionService.ToDomainClass", $"Unknown class {klasse}")
        };
    }

    private static Currency ToDomainCurrency(this Waehrung currency)
    {
        return currency switch
        {
            Waehrung.EUR => Currency.Euro,
            _ => throw new BahnDeException("ConnectionService.ToDomainCurrency", $"Unknown currency {currency}")
        };
    }

    private static bool? GetBikeCarriageInformation(this Fahrradmitnahme? status)
    {
        if (status is null) return null;
        
        return status switch
        {
            Fahrradmitnahme.NICHT_BEWERTBAR => null,
            Fahrradmitnahme.NICHT_MOEGLICH => false,
            Fahrradmitnahme.MOEGLICH => true,
            _ => throw new BahnDeException("ConnectionService.GetBikeCarriageInformation", $"Unknown bike carriage status: {status}")
        };
    }

    private static TransportSection ToDomain(
        this VerbindungsAbschnitt abschnitt, 
        Dictionary<string, TrainRunEntity> trainRunMapping,
        out TrainRunEntity? newTrainRun)
    {
        var trains = TrainInformationFactory.Create(
            abschnitt.Verkehrsmittel.MittelText!, 
            abschnitt.Verkehrsmittel.LangText!);

        newTrainRun = null;
        var trainRunExists = trainRunMapping.TryGetValue(abschnitt.JourneyId!, out var trainRun);

        var trainRunId = trainRun?.Id;
        
        StationName? destination = null;

        if (abschnitt.Verkehrsmittel.Richtung is not null)
        {
            var destinationResult = StationName.Create(abschnitt.Verkehrsmittel.Richtung);
            if (!destinationResult.HasFailed)
            {
                destination = destinationResult.Value;
            }
        }
        
        if (!trainRunExists)
        {
            trainRunId = TrainRunId.CreateNew();
            var bahnId = new JourneyId(abschnitt.JourneyId!);
            
            newTrainRun = new TrainRunEntity(trainRunId, bahnId, trains[0], destination);
            trainRunMapping.Add(abschnitt.JourneyId!, newTrainRun);
        }
        else if(trains[0].Number is null && trainRun!.TrainInfos.Number is not null)
        {
            trains[0] = trains[0].UpdateTrainNumber(trainRun.TrainInfos.Number);
        }
        
        return TransportSection.Create(
            abschnitt.Auslastungsmeldungen.GetDomainDemand(),
            abschnitt.GetDomainSectionMessages(),
            trainRunId!,
            trains,
            destination,
            abschnitt.GetCateringInformation(trains[0]),
            abschnitt.GetBikeCarriageInformation(),
            abschnitt.Halte.Select(h => h.ToDomain()).ToList());
    }

    private static CateringInformation GetCateringInformation(this VerbindungsAbschnitt abschnitt, TrainInformation trainInformation)
    {
        IPartialValidityStopInfos t = new Halt();
        return TrainInformationFactory.CreateCateringInformation(
            abschnitt.Verkehrsmittel.Zugattribute,
            trainInformation.Product,
            abschnitt.Halte);
    }
    
    private static BikeCarriage GetBikeCarriageInformation(this VerbindungsAbschnitt abschnitt)
    {
        return TrainInformationFactory.CreateBikeCarriageInformation(
            abschnitt.Verkehrsmittel.Zugattribute,
            abschnitt.HimMeldungen,
            abschnitt.Halte);
    }
    
    private static Stop ToDomain(this Halt halt)
    {
        Platform? platform = null;
        if (halt.Gleis is not null)
        {
            var planned =  halt.Gleis!;
            var real = halt.EzGleis;

            var type = PlatformType.Platform;
            
            platform = new Platform(planned, real, type);
        }
        
        var additional = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.additional");
        var cancelled = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.cancelled");
        var exitOnly = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.entry.disabled");
        var entryOnly = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.exit.disabled");
        
        DepartureTime? departureTime = null;
        if (halt.AbfahrtsZeitpunkt is not null)
        {
            var planned = halt.AbfahrtsZeitpunkt.ConvertToDateTime()!.Value;
            var real = halt.EzAbfahrtsZeitpunkt.ConvertToDateTime();
            
            departureTime = new DepartureTime(planned, real);
        }
        
        ArrivalTime? arrivalTime = null;
        if (halt.AnkunftsZeitpunkt is not null)
        {
            var planned = halt.AnkunftsZeitpunkt.ConvertToDateTime()!.Value;
            var real = halt.EzAnkunftsZeitpunkt.ConvertToDateTime();
            
            arrivalTime = new ArrivalTime(planned, real);
        }
        //TODO: get real station id from database
        return new Stop(
            StationId.CreateNew(),
            new StopIndex(halt.RouteIdx),
            platform,
            halt.Auslastungsmeldungen.GetDomainDemand(),
            additional,
            cancelled,
            exitOnly,
            entryOnly,
            departureTime,
            arrivalTime,
            halt.GetDomainMessages());
    }
}