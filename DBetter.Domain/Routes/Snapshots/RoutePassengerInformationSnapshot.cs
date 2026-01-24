using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.Entities;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes.Snapshots;

public record RoutePassengerInformationSnapshot(PassengerInformationId Id, StopIndex FromStopIndex, StopIndex ToStopIndex);