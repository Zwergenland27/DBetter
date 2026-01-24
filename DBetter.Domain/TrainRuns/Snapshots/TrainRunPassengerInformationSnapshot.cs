using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns.Snapshots;

public record TrainRunPassengerInformationSnapshot(PassengerInformationId Id, StopIndex FromStopIndex, StopIndex ToStopIndex);