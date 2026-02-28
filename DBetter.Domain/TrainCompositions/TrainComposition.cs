using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCompositions.Snapshots;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public class TrainComposition : AggregateRoot<TrainCompositionId>
{
    private List<FormationVehicle> _vehicles;
    
    public TrainRunId TrainRun { get; private init; }
    
    public TrainFormationSource Source { get; private set; }
    
    public IReadOnlyList<FormationVehicle> Vehicles => _vehicles.AsReadOnly();
    
    private TrainComposition() : base(null!){}

    private TrainComposition(
        TrainCompositionId id,
        List<FormationVehicle> vehicles,
        TrainRunId trainRun,
        TrainFormationSource source) : base(id)
    {
        TrainRun = trainRun;
        Source = source;
        _vehicles = vehicles;
    }

    public static TrainComposition CreateFromRealtime(TrainRunId trainRunId, List<FormationVehicleSnapshot> formations)
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
        return new TrainComposition(TrainCompositionId.CreateNew(), formationSections, trainRunId, TrainFormationSource.RealTime);
    }

    public static TrainComposition CreateFromPrediction(TrainRunId trainRunId, TrainComposition predictedComposition)
    {
        var formations = predictedComposition.Vehicles
            .Select(f => new FormationVehicle(f))
            .ToList(); 
        return new TrainComposition(TrainCompositionId.CreateNew(), formations, trainRunId, TrainFormationSource.Prediction);
    }
    
    public static TrainComposition CreateFromPlanned(TrainRunId trainRunId, List<FormationVehicleSnapshot> formations)
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
        return new TrainComposition(TrainCompositionId.CreateNew(), formationSections, trainRunId, TrainFormationSource.SeatingPlan);
    }

    public void Update(List<FormationVehicleSnapshot> formations)
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

        Source = TrainFormationSource.RealTime;
    }

    public string CalculateIdentifier()
    {
        return string.Join("|", _vehicles.Select(v => $"{v.VehicleId};{v.FromStation};{v.ToStation}"));
    }
}