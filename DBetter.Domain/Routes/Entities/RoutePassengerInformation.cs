using DBetter.Domain.Abstractions;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes.Entities;

public class RoutePassengerInformation : Entity<RoutePassengerInformationId>
{
    public PassengerInformationId InformationId { get; private init; }
    
    public StopIndex FromStopIndex { get; private set; }
    
    public StopIndex ToStopIndex { get; private set; }

    internal RoutePassengerInformation() : base(null!){}

    private RoutePassengerInformation(
        RoutePassengerInformationId id,
        PassengerInformationId informationId,
        StopIndex fromStopIndex,
        StopIndex toStopIndex) : base(id)
    {
        InformationId = informationId;
        FromStopIndex = fromStopIndex;
        ToStopIndex = toStopIndex;
    }

    public static RoutePassengerInformation Create(
        PassengerInformationId informationId,
        StopIndex fromStopIndex,
        StopIndex toStopIndex)
    {
        return new  RoutePassengerInformation(RoutePassengerInformationId.CreateNew(), informationId, fromStopIndex, toStopIndex);
    }
}