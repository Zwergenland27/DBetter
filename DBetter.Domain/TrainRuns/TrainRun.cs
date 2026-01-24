using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainRuns.Entities;
using DBetter.Domain.TrainRuns.Events;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

/// <summary>
/// Complete train run of a vehicle
/// </summary>
public class TrainRun : AggregateRoot<TrainRunId>
{
    private List<TrainRunPassengerInformation> _passengerInformation = [];
    
    public BahnJourneyId JourneyId { get; private init; }
    
    public IReadOnlyList<TrainRunPassengerInformation> PassengerInformation => _passengerInformation.AsReadOnly();
    
    public CateringInformation Catering  { get; private set; }
    
    public BikeCarriageInformation BikeCarriage { get; private set; }
    
    public ServiceInformation ServiceInformation { get; private set; }
    
    private TrainRun() : base(null!){}

    private TrainRun(
        TrainRunId id,
        BahnJourneyId journeyId,
        List<TrainRunPassengerInformation> passengerInformation,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation) : base(id)
    {
        JourneyId = journeyId;
        _passengerInformation = passengerInformation;
        ServiceInformation = serviceInformation;
        Catering = cateringInformation;
        BikeCarriage = bikeCarriageInformation;
    }
    
    public static TrainRun Create(
        BahnJourneyId journeyId,
        List<TrainRunPassengerInformationSnapshot> passengerInformation,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation)
    {
        var trainRun = new TrainRun(
            TrainRunId.CreateNew(),
            journeyId,
            [],
            serviceInformation,
            cateringInformation,
            bikeCarriageInformation);

        trainRun.ReconcilePassengerInformation(passengerInformation);
        
        if (trainRun.ServiceInformation.TransportCategory is
            TransportCategory.HighSpeedTrain or
            TransportCategory.FastTrain or
            TransportCategory.RegionalTrain or
            TransportCategory.SuburbanTrain)
        {
            trainRun.RaiseDomainEvent(new TrainRunScrapingScheduledEvent(trainRun.Id));
        }
        return trainRun;
    }

    public void Update(CateringInformation cateringInformation)
    {
        Catering = Catering.Update(cateringInformation);
    }
    
    public void Update(BikeCarriageInformation bikeCarriageInformation)
    {
        BikeCarriage = BikeCarriage.Update(bikeCarriageInformation);
    }

    public void ReconcilePassengerInformation(List<TrainRunPassengerInformationSnapshot> incomingPassengerInformation)
    {
        var remainingPassengerInformation = incomingPassengerInformation;
        foreach (var existingPassengerInformation in _passengerInformation)
        {
            var incoming = remainingPassengerInformation
                .FirstOrDefault(im => im.Id == existingPassengerInformation.InformationId);
            if (incoming is not null)
            {
                remainingPassengerInformation.Remove(incoming);
            }
        }

        foreach (var passengerInformation in remainingPassengerInformation)
        {
            var trainRunPassengerInformation = TrainRunPassengerInformation.Create(passengerInformation.Id, passengerInformation.FromStopIndex, passengerInformation.ToStopIndex);
            _passengerInformation.Add(trainRunPassengerInformation);
        }
    }

    public void Update(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }
}