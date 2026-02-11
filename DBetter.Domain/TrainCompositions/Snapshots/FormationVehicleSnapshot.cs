using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.TrainCompositions.Snapshots;

public record FormationVehicleSnapshot(StationId FromStop, StationId ToStop, VehicleId Vehicle);