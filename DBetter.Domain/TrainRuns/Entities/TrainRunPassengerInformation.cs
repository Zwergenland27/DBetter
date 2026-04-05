using DBetter.Domain.Abstractions;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns.Entities;

public class TrainRunPassengerInformation : Entity<TrainRunPassengerInformationId>
{
    public PassengerInformationId InformationId { get; private init; }
    
    public StopIndex FromStopIndex { get; private set; }
    
    public StopIndex ToStopIndex { get; private set; }
    

    internal TrainRunPassengerInformation(
        TrainRunPassengerInformationId id,
        PassengerInformationId informationId,
        StopIndex fromStopIndex,
        StopIndex toStopIndex) : base(id)
    {
        InformationId = informationId;
        FromStopIndex = fromStopIndex;
        ToStopIndex = toStopIndex;
    }

    internal static TrainRunPassengerInformation Create(
        TrainRunPassengerInformationId id,
        PassengerInformationId informationId,
        StopIndex fromStopIndex,
        StopIndex toStopIndex)
    {
        return new  TrainRunPassengerInformation(id, informationId, fromStopIndex, toStopIndex);
    }
}