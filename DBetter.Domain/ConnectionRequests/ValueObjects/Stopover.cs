using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record Stopover(StationId StationId, int StayMinutes);