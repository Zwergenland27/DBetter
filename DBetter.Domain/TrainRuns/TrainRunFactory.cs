using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public class TrainRunFactory
{
    public static TrainRun Create(
        TrainCirculation trainCirculation,
        BahnJourneyId journeyId,
        List<TrainRunPassengerInformationSnapshot> passengerInformation,
        BikeCarriageInformation bikeCarriage,
        CateringInformation catering)
    {
        var isRailway = trainCirculation.ServiceInformation.TransportCategory is
            TransportCategory.HighSpeedTrain or
            TransportCategory.FastTrain or
            TransportCategory.RegionalTrain or
            TransportCategory.SuburbanTrain;
        
        var trainRun = TrainRun.Create(
            trainCirculation.Id,
            journeyId,
            journeyId.OperatingDay,
            catering,
            bikeCarriage,
            isRailway);
        
        trainRun.ReconcilePassengerInformation(passengerInformation);
        return trainRun;
    }
}