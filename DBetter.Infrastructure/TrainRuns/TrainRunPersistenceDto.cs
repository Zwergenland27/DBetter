using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;

namespace DBetter.Infrastructure.TrainRuns;

public class TrainRunPersistenceDto: IPersistenceDto<TrainRun>
{
    public required Guid Id { get; init; }
    
    public required Guid TrainCirculationId { get; init; }
    
    public required DateOnly OperatingDay { get; init; }
    
    public required string BahnJourneyId { get; set; }
    
    public required int Catering_Type { get; set; }
    
    public required int Catering_FromStopIndex { get; set; }
    
    public required int Catering_ToStopIndex { get; set; }
    
    public required int BikeCarriage_Status { get; set; }
    
    public required int BikeCarriage_FromStopIndex { get; set; }
    
    public required int BikeCarriage_ToStopIndex { get; set; }
    
    public required List<TrainRunPassengerInformationPersistenceDto> PassengerInformation { get; set; }

    public static TrainRunPersistenceDto FromDomain(TrainRun domain)
    {
        return new TrainRunPersistenceDto
        {
            Id = domain.Id.Value,
            TrainCirculationId = domain.CirculationId.Value,
            OperatingDay = domain.OperatingDay.Date,
            BahnJourneyId = domain.JourneyId.Value,
            Catering_Type = (int)domain.Catering.Type,
            Catering_FromStopIndex = domain.Catering.FromStopIndex.Value,
            Catering_ToStopIndex = domain.Catering.ToStopIndex.Value,
            BikeCarriage_Status = (int)domain.BikeCarriage.Status,
            BikeCarriage_FromStopIndex = domain.BikeCarriage.FromStopIndex.Value,
            BikeCarriage_ToStopIndex = domain.BikeCarriage.ToStopIndex.Value,
            PassengerInformation = domain.PassengerInformation
                .Select(TrainRunPassengerInformationPersistenceDto.FromDomain).ToList()
        };
    }
    
    public TrainRun ToDomain()
    {
        return new TrainRun(
            new TrainRunId(Id),
            new TrainCirculationId(TrainCirculationId),
            new OperatingDay(OperatingDay),
            Domain.TrainRuns.Snapshots.BahnJourneyId.Create(BahnJourneyId),
            PassengerInformation.Select(dto => dto.ToDomain()).ToList(),
            new CateringInformation(
                (CateringType) Catering_Type,
                new StopIndex(Catering_FromStopIndex),
                new StopIndex(Catering_ToStopIndex)),
            new BikeCarriageInformation(
                (BikeCarriageStatus)BikeCarriage_Status,
                new StopIndex(BikeCarriage_FromStopIndex),
                new StopIndex(BikeCarriage_ToStopIndex))
            );
    }

    public void Apply(TrainRun domain)
    {
        BahnJourneyId = domain.JourneyId.Value;
        Catering_Type = (int)domain.Catering.Type;
        Catering_FromStopIndex = domain.Catering.FromStopIndex.Value;
        Catering_ToStopIndex = domain.Catering.ToStopIndex.Value;
        BikeCarriage_Status = (int)domain.BikeCarriage.Status;
        BikeCarriage_FromStopIndex = domain.BikeCarriage.FromStopIndex.Value;
        BikeCarriage_ToStopIndex = domain.BikeCarriage.ToStopIndex.Value;
        PassengerInformation.Synchronize(
            domain.PassengerInformation,
            dto => dto.Id,
            d => d.Id.Value,
            TrainRunPassengerInformationPersistenceDto.FromDomain);
    }
}