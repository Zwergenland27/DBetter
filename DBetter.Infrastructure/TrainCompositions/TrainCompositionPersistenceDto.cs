using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;
using DBetter.Infrastructure.Postgres;

namespace DBetter.Infrastructure.TrainCompositions;

public class TrainCompositionPersistenceDto: IPersistenceDto<TrainComposition>
{
    public required Guid Id { get; init; }
    
    public required Guid TrainRunId { get; init; }
    
    public required int Source { get; set; }
    
    public required List<FormationVehiclePersistenceDto> Vehicles { get; set; }
    
    public required DateTime DepartureTime { get; init; }
    
    public required DateTime LastUpdate { get; set; }
    
    //TODO: Rename to NextUpdateInterval
    public required int? CurrentUpdateInterval { get; set; }

    public static TrainCompositionPersistenceDto FromDomain(TrainComposition trainComposition)
    {
        return new()
        {
            Id = trainComposition.Id.Value,
            TrainRunId = trainComposition.TrainRun.Value,
            Source = (int)trainComposition.Source,
            DepartureTime = trainComposition.DepartureTime,
            LastUpdate = trainComposition.LastUpdate,
            CurrentUpdateInterval = trainComposition.CurrentUpdateInterval,
            Vehicles = trainComposition.Vehicles.Select(FormationVehiclePersistenceDto.FromDomain).ToList()
        };
    }
    
    public TrainComposition ToDomain()
    {
        return new TrainComposition(
            new TrainCompositionId(Id),
            Vehicles.Select(v => v.ToDomain()).ToList(),
            new TrainRunId(TrainRunId),
            (TrainFormationSource) Source,
            DepartureTime,
            LastUpdate,
            CurrentUpdateInterval);
    }

    public void Apply(TrainComposition trainComposition)
    {
        Source = (int)trainComposition.Source;
        LastUpdate = trainComposition.LastUpdate;
        Vehicles.Synchronize(
            trainComposition.Vehicles,
            dto => dto.Id,
            domain => domain.Id.Value,
            FormationVehiclePersistenceDto.FromDomain);
    }
}

public class FormationVehiclePersistenceDto: IPersistenceDto<FormationVehicle>
{
    public byte Id { get; init; }
    
    public Guid VehicleId { get; set; }
    
    public Guid FromStationId { get; set; } 
    
    public Guid ToStationId { get; set; }

    public static FormationVehiclePersistenceDto FromDomain(FormationVehicle formationVehicle)
    {
        return new FormationVehiclePersistenceDto
        {
            Id = formationVehicle.Id.Value,
            VehicleId = formationVehicle.VehicleId.Value,
            FromStationId = formationVehicle.FromStation.Value,
            ToStationId = formationVehicle.ToStation.Value,
        };
    }
    
    public FormationVehicle ToDomain()
    {
        return new FormationVehicle(
            new FormationVehicleId(Id),
            new VehicleId(VehicleId),
            new StationId(FromStationId),
            new StationId(ToStationId));
    }

    public void Apply(FormationVehicle formationVehicle)
    {
        VehicleId = formationVehicle.VehicleId.Value;
        FromStationId = formationVehicle.FromStation.Value;
        ToStationId = formationVehicle.ToStation.Value;
    }
}