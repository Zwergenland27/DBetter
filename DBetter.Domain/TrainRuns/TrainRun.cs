using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCirculations.ValueObjects;
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
    
    public TrainCirculationId CirculationId { get; private init; }
    
    public OperatingDay OperatingDay { get; private init; }
    
    public BahnJourneyId JourneyId { get; private init; }
    
    public IReadOnlyList<TrainRunPassengerInformation> PassengerInformation => _passengerInformation.AsReadOnly();
    
    public CateringInformation Catering  { get; private set; }
    
    public BikeCarriageInformation BikeCarriage { get; private set; }

    internal TrainRun(
        TrainRunId id,
        TrainCirculationId circulationId,
        OperatingDay operatingDay,
        BahnJourneyId journeyId,
        List<TrainRunPassengerInformation> passengerInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation) : base(id)
    {
        CirculationId = circulationId;
        JourneyId = journeyId;
        OperatingDay = operatingDay;
        _passengerInformation = passengerInformation;
        Catering = cateringInformation;
        BikeCarriage = bikeCarriageInformation;
    }
    
    internal static TrainRun Create(
        TrainCirculationId circulationId,
        BahnJourneyId journeyId,
        OperatingDay operatingDay,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation,
        bool isRailway)
    {
        var trainRun = new TrainRun(
            TrainRunId.CreateNew(),
            circulationId,
            operatingDay,
            journeyId,
            [],
            cateringInformation,
            bikeCarriageInformation);

        if (isRailway)
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
}