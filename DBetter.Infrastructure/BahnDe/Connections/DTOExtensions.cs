using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
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
        out List<TrainRunEntity> newTrainRuns,
        Dictionary<string, Station> stationMapping,
        out List<Station> newStations)
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
        newStations = [];

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
            
            sections.Add(abschnitt.ToDomain(trainRunMapping, out var newTrainRun, stationMapping, out var newSectionStations));

            if (newTrainRun is not null)
            {
                newTrainRuns.Add(newTrainRun);
            }
            
            newStations.AddRange(newSectionStations);
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
        out TrainRunEntity? newTrainRun,
        Dictionary<string, Station> stationMapping,
        out List<Station> newStations)
    {
        var trains = TrainInformationFactory.Create(
            abschnitt.Verkehrsmittel.MittelText!, 
            abschnitt.Verkehrsmittel.LangText!);

        newTrainRun = null;
        var trainRunExists = trainRunMapping.TryGetValue(abschnitt.JourneyId!, out var trainRun);

        var trainRunId = trainRun?.Id;
        
        if (!trainRunExists)
        {
            trainRunId = TrainRunId.CreateNew();
            var bahnId = new JourneyId(abschnitt.JourneyId!);
            
            newTrainRun = new TrainRunEntity(trainRunId, bahnId, trains[0]);
            trainRunMapping.Add(abschnitt.JourneyId!, newTrainRun);
        }

        List<Station> tmpNewStations = [];
        
        var section = TransportSection.Create(
            abschnitt.Auslastungsmeldungen.GetDomainDemand(),
            abschnitt.GetDomainSectionMessages(),
            trainRunId!,
            trains,
            abschnitt.GetCateringInformation(trains[0]),
            abschnitt.GetBikeCarriageInformation(),
            abschnitt.Halte.Select(h =>
            {
                var stop = h.ToDomain(stationMapping, out var newStation);

                if (newStation is not null)
                {
                    tmpNewStations.Add(newStation);
                }
                
                return stop;
            }).ToList());
        
        newStations = tmpNewStations;
        return section;
    }

    private static CateringInformation GetCateringInformation(this VerbindungsAbschnitt abschnitt, TrainInformation trainInformation)
    {
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
    
    private static Stop ToDomain(this Halt halt, Dictionary<string, Station> stationMapping, out Station? newStation)
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
        
        newStation = null;
        var stationExists =  stationMapping.TryGetValue(halt.ExtId, out var station);

        var stationId = station?.Id;
        
        if (!stationExists)
        {
            stationId = StationId.CreateNew();
            
            StationInfoId?  stationInfoId = null;
            if (halt.BahnhofsInfoId is not null)
            {
                stationInfoId = StationInfoId.Create(halt.BahnhofsInfoId).Value;
            }
            
            newStation = new Station(
                stationId,
                EvaNumber.Create(halt.ExtId).Value,
                StationName.Create(halt.Name).Value,
                null,
                stationInfoId);
            
            stationMapping.Add(halt.ExtId, newStation);
        }
        
        return new Stop(
            stationId!,
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