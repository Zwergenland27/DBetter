using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record Stopover(EvaNumber Stop, int StayMinutes);