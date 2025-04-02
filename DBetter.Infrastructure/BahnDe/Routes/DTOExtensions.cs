using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;
using DBetter.Infrastructure.BahnDe.TrainRuns.Entities;

namespace DBetter.Infrastructure.BahnDe.TrainRuns;

public static class DTOExtensions
{
    public static Route ToDomain(this Fahrt fahrt, TrainRunEntity trainRunEntity, Dictionary<string, Station> stationMapping, out List<Station> newStations)
    {
        List<Station> tmpNewStations = [];
        
        var trainRun = new Route(
            trainRunEntity.Id,
            fahrt.GetDomainSectionMessages(),
            fahrt.GetTrainInfos(trainRunEntity),
            fahrt.GetCateringInformation(trainRunEntity),
            fahrt.GetBikeCarriageInformation(),
            fahrt.Halte.Select(h =>
            {
                var stop = h.ToDomain(stationMapping, out var newStation);

                if (newStation is not null)
                {
                    tmpNewStations.Add(newStation);
                }
                
                return stop;
            }).ToList());
        
        newStations = tmpNewStations;
        return trainRun;
    }

    private static RouteInformation GetTrainInfos(this Fahrt fahrt, TrainRunEntity trainRunEntity)
    {
        var nummer = fahrt.Halte
            .First()
            .Nummer;
        
        var trainNumber = TrainInformationFactory.GetTrainNumber(nummer);
        
        if (trainRunEntity.RouteInfos.Number is null && trainNumber is not null)
        {
            trainRunEntity.UpdateTrainNumber(trainNumber);
        }

        return new RouteInformation(
            trainRunEntity.RouteInfos.Product,
            trainRunEntity.RouteInfos.Line,
            trainRunEntity.RouteInfos.Number);
    }

    private static CateringInformation GetCateringInformation(this Fahrt fahrt, TrainRunEntity trainRun)
    {
        return TrainInformationFactory.CreateCateringInformation(
            fahrt.Zugattribute,
            trainRun.RouteInfos.Product,
            fahrt.Halte);
    }

    private static BikeCarriage GetBikeCarriageInformation(this Fahrt fahrt)
    {
        return TrainInformationFactory.CreateBikeCarriageInformation(
            fahrt.Zugattribute,
            fahrt.HimMeldungen,
            fahrt.Halte);
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
        
        TravelTime? arrivalTime = null;
        if (halt.AnkunftsZeitpunkt is not null)
        {
            var planned = halt.AnkunftsZeitpunkt.ConvertToDateTime()!.Value;
            var real = halt.EzAnkunftsZeitpunkt.ConvertToDateTime();
            
            arrivalTime = new TravelTime(planned, real);
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