namespace DBetter.Infrastructure.TrainCompositions;

public class TrainCompositionPersistenceDto//: IPersistenceDto<TrainComposition>
{
    // public required Guid Id { get; init; }
    //
    // public required Guid TrainRunId { get; init; }
    //
    // public required int Source { get; set; }
    //
    // public required List<FormationVehiclePersistenceDto> Vehicles { get; set; }
    //
    // public required DateTime DepartureTime { get; init; }
    //
    // public required DateTime LastUpdate { get; set; }
    //
    // //TODO: Rename to NextUpdateInterval
    // public required int? CurrentUpdateInterval { get; set; }
    //
    // public static TrainCompositionPersistenceDto FromDomain(TrainComposition trainComposition)
    // {
    //     return new()
    //     {
    //         Id = trainComposition.Id.Value,
    //         TrainRunId = trainComposition.TrainRunId.Value,
    //         Source = (int)trainComposition.Source,
    //         DepartureTime = trainComposition.DepartureTime,
    //         LastUpdate = trainComposition.LastUpdate,
    //         CurrentUpdateInterval = trainComposition.CurrentUpdateInterval,
    //         Vehicles = trainComposition.ResolvedParts.Select(FormationVehiclePersistenceDto.FromDomain).ToList()
    //     };
    // }
    //
    // public TrainComposition ToDomain()
    // {
    //     return new TrainComposition(
    //         new TrainCompositionId(Id),
    //         Vehicles.Select(v => v.ToDomain()).ToList(),
    //         new TrainRunId(TrainRunId),
    //         (TrainFormationSource) Source,
    //         DepartureTime,
    //         LastUpdate,
    //         CurrentUpdateInterval);
    // }
    //
    // public void Apply(TrainComposition trainComposition)
    // {
    //     Source = (int)trainComposition.Source;
    //     LastUpdate = trainComposition.LastUpdate;
    //     Vehicles.Synchronize(
    //         trainComposition.ResolvedParts,
    //         dto => dto.Id,
    //         domain => domain.Id.Value,
    //         FormationVehiclePersistenceDto.FromDomain);
    // }
}
//
// public class FormationVehiclePersistenceDto: IPersistenceDto<TrainPart>
// {
//     public byte Id { get; init; }
//     
//     public Guid VehicleId { get; set; }
//     
//     public Guid FromStationId { get; set; } 
//     
//     public Guid ToStationId { get; set; }
//
//     public static FormationVehiclePersistenceDto FromDomain(TrainPart trainPart)
//     {
//         return new FormationVehiclePersistenceDto
//         {
//             Id = trainPart.Id.Value,
//             VehicleId = trainPart.VehicleId.Value,
//             FromStationId = trainPart.FromStation.Value,
//             ToStationId = trainPart.ToStation.Value,
//         };
//     }
//     
//     public TrainPart ToDomain()
//     {
//         return new TrainPart(
//             new TrainPartId(Id),
//             new VehicleId(VehicleId),
//             new StationId(FromStationId),
//             new StationId(ToStationId));
//     }
//
//     public void Apply(TrainPart trainPart)
//     {
//         VehicleId = trainPart.VehicleId.Value;
//         FromStationId = trainPart.FromStation.Value;
//         ToStationId = trainPart.ToStation.Value;
//     }
// }