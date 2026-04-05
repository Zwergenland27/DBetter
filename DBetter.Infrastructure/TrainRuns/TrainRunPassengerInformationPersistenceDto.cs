using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainRuns.Entities;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;

namespace DBetter.Infrastructure.TrainRuns;

public class TrainRunPassengerInformationPersistenceDto: IPersistenceDto<TrainRunPassengerInformation>
{
    public required ushort Id { get; init; }
    
    public required Guid PassengerInformationId { get; set; }
    
    public required int FromStopIndex { get; set; }
    
    public required int ToStopIndex { get; set; }

    public static TrainRunPassengerInformationPersistenceDto FromDomain(TrainRunPassengerInformation domain)
    {
        return new TrainRunPassengerInformationPersistenceDto
        {
            Id = domain.Id.Value,
            PassengerInformationId = domain.InformationId.Value,
            FromStopIndex = domain.FromStopIndex.Value,
            ToStopIndex = domain.ToStopIndex.Value,
        };
    }
    
    public TrainRunPassengerInformation ToDomain()
    {
        return new TrainRunPassengerInformation(
            new TrainRunPassengerInformationId(Id),
            new PassengerInformationId(PassengerInformationId),
            new StopIndex(FromStopIndex),
            new StopIndex(ToStopIndex));
    }

    public void Apply(TrainRunPassengerInformation domain)
    {
        PassengerInformationId = domain.InformationId.Value;
        FromStopIndex = domain.FromStopIndex.Value;
        ToStopIndex = domain.ToStopIndex.Value;
    }
}