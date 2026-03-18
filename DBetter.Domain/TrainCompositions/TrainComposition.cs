using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCompositions.Events;
using DBetter.Domain.TrainCompositions.Snapshots;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public class TrainComposition : AggregateRoot<TrainCompositionId>
{
    private static readonly int[] UpdateIntervalHours = [8, 4, 2, 0];
    
    private List<FormationVehicle> _vehicles;
    
    public TrainRunId TrainRun { get; private init; }
    
    public TrainFormationSource Source { get; private set; }
    
    public IReadOnlyList<FormationVehicle> Vehicles => _vehicles.AsReadOnly();
    
    public DateTime DepartureTime { get; private init; }
    
    public DateTime LastUpdate { get; private set; }
    
    //TODO: Rename to NextUpdateInterval
    public int? CurrentUpdateInterval { get; private set; }
    
    private TrainComposition() : base(null!){}

    private TrainComposition(
        TrainCompositionId id,
        List<FormationVehicle> vehicles,
        TrainRunId trainRun,
        TrainFormationSource source,
        TravelTime departureTime,
        DateTime lastUpdate) : base(id)
    {
        TrainRun = trainRun;
        Source = source;
        _vehicles = vehicles;
        DepartureTime = departureTime.Planned;
        LastUpdate = lastUpdate;
    }

    public static TrainComposition CreateFromRealtime(TrainRunId trainRunId, TravelTime departureTime, List<FormationVehicleSnapshot> formations)
    {
        var formationSections = new List<FormationVehicle>();
        byte formationIndex = 0;
        foreach (var formation in formations)
        {
            formationSections.Add(new FormationVehicle(
                new FormationVehicleId(formationIndex),
                formation.Vehicle,
                formation.FromStop,
                formation.ToStop));
            formationIndex++;
        }
        var trainComposition = new TrainComposition(TrainCompositionId.CreateNew(), formationSections, trainRunId, TrainFormationSource.RealTime, departureTime, DateTime.UtcNow);
        trainComposition.RaiseDomainEvent(new TrainCompositionUpdated(trainComposition.TrainRun));
        return trainComposition;
    }

    public static TrainComposition CreateFromPrediction(TrainRunId trainRunId, TravelTime departureTime, TrainComposition predictedComposition)
    {
        var formations = predictedComposition.Vehicles
            .Select(f => new FormationVehicle(f))
            .ToList(); 
        var trainComposition = new TrainComposition(TrainCompositionId.CreateNew(), formations, trainRunId, TrainFormationSource.Prediction, departureTime, DateTime.UtcNow);
        trainComposition.ScheduleUpdate();
        trainComposition.RaiseDomainEvent(new TrainCompositionUpdated(trainComposition.TrainRun));
        return trainComposition;
    }
    
    public static TrainComposition CreateFromPlanned(TrainRunId trainRunId, TravelTime departureTime, List<FormationVehicleSnapshot> formations)
    {
        var formationSections = new List<FormationVehicle>();
        byte formationIndex = 0;
        foreach (var formation in formations)
        {
            formationSections.Add(new FormationVehicle(
                new FormationVehicleId(formationIndex),
                formation.Vehicle,
                formation.FromStop,
                formation.ToStop));
            formationIndex++;
        }
        var trainComposition = new TrainComposition(TrainCompositionId.CreateNew(), formationSections, trainRunId, TrainFormationSource.SeatingPlan, departureTime, DateTime.UtcNow);
        trainComposition.ScheduleUpdate();
        trainComposition.RaiseDomainEvent(new TrainCompositionUpdated(trainComposition.TrainRun));
        return trainComposition;
    }

    public static TrainComposition CreateNotFound(TrainRunId trainRunId, TravelTime departureTime)
    {
        var trainComposition = new TrainComposition(TrainCompositionId.CreateNew(), [], trainRunId, TrainFormationSource.None, departureTime, DateTime.UtcNow);
        trainComposition.ScheduleUpdate();
        trainComposition.RaiseDomainEvent(new TrainCompositionUpdated(trainComposition.TrainRun));
        return trainComposition;
    }

    public bool IsNextCheckAllowed => Source is not TrainFormationSource.RealTime &&
                                      DateTime.UtcNow - LastUpdate > TimeSpan.FromMinutes(60);

    public void UpdateFromRealTime(List<FormationVehicleSnapshot> formations)
    {
        UpdateFormations(formations);
        //TODO: Cancel schedules
        Source = TrainFormationSource.RealTime;
        RaiseDomainEvent(new TrainCompositionUpdated(TrainRun));
    }

    public void UpdateFromPlanned(List<FormationVehicleSnapshot> formations)
    {
        if (Source is TrainFormationSource.RealTime) return;
        ScheduleUpdate();
        UpdateFormations(formations);
        Source = TrainFormationSource.SeatingPlan;
        RaiseDomainEvent(new TrainCompositionUpdated(TrainRun));
    }

    public void UpdateFromPrediction(TrainComposition predictedComposition)
    {
        if (Source is TrainFormationSource.RealTime or TrainFormationSource.SeatingPlan) return;
        ScheduleUpdate();
        _vehicles.Clear();

        var formations = predictedComposition.Vehicles
            .Select(f => new FormationVehicle(f));

        foreach (var formation in formations)
        {
            _vehicles.Add(formation);
        }
        LastUpdate = DateTime.UtcNow;
        Source = TrainFormationSource.Prediction;
        RaiseDomainEvent(new TrainCompositionUpdated(TrainRun));
    }

    private void UpdateFormations(List<FormationVehicleSnapshot> formations)
    {
        _vehicles.Clear();
        byte formationIndex = 0;
        foreach (var formation in formations)
        {
            _vehicles.Add(new FormationVehicle(
                new FormationVehicleId(formationIndex),
                formation.Vehicle,
                formation.FromStop,
                formation.ToStop));
            formationIndex++;
        }
        
        LastUpdate = DateTime.UtcNow;
    }

    public string CalculateIdentifier()
    {
        return string.Join("|", _vehicles.Select(v => $"{v.VehicleId};{v.FromStation};{v.ToStation}"));
    }

    public void ScheduleUpdate()
    {
        if (Source is TrainFormationSource.RealTime) return;
        if (CurrentUpdateInterval is null)
        {
            CurrentUpdateInterval = 0;
        }

        DateTime scheduledAt;
        do
        {
            if(CurrentUpdateInterval >= UpdateIntervalHours.Length)
            {
                return;
            }
            var hoursBeforeDeparture = -UpdateIntervalHours[CurrentUpdateInterval.Value];
            scheduledAt = DepartureTime.AddHours(hoursBeforeDeparture);
            CurrentUpdateInterval++;
        } while (DateTime.UtcNow - scheduledAt > TimeSpan.FromHours(2));
        
            
        RaiseDomainEvent(new TrainCompositionUpdateScheduled(TrainRun, scheduledAt));
    }
}