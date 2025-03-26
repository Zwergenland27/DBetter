using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;
using DBetter.Infrastructure.BahnDe.TrainRuns.Entities;

namespace DBetter.Infrastructure.BahnDe.TrainRuns;

public static class DTOExtensions
{
    public static TrainRun ToDomain(this Fahrt fahrt, TrainRunEntity trainRun)
    {
        return new TrainRun(
            trainRun.Id,
            fahrt.GetDomainSectionMessages(),
            fahrt.GetTrainInfos(trainRun),
            fahrt.GetCateringInformation(trainRun),
            fahrt.GetBikeCarriageInformation(),
            fahrt.Halte.Select(ToDomain).ToList());
    }

    private static TrainInformation GetTrainInfos(this Fahrt fahrt, TrainRunEntity trainRun)
    {
        var nummer = fahrt.Halte
            .First()
            .Nummer;
        
        var trainNumber = TrainInformationFactory.GetTrainNumber(nummer);
        
        if (trainRun.TrainInfos.Number is null && trainNumber is not null)
        {
            trainRun.UpdateTrainNumber(trainNumber);
        }

        return new TrainInformation(
            trainRun.TrainInfos.Product,
            trainRun.TrainInfos.Line,
            trainRun.TrainInfos.Number);
    }

    private static CateringInformation GetCateringInformation(this Fahrt fahrt, TrainRunEntity trainRun)
    {
        return TrainInformationFactory.CreateCateringInformation(
            fahrt.Zugattribute,
            trainRun.TrainInfos.Product,
            fahrt.Halte);
    }

    private static BikeCarriage GetBikeCarriageInformation(this Fahrt fahrt)
    {
        return TrainInformationFactory.CreateBikeCarriageInformation(
            fahrt.Zugattribute,
            fahrt.HimMeldungen,
            fahrt.Halte);
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