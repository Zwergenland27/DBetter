using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record ConnectionOptions(
    Class Class,
    int MaxTransfers,
    int MinTransferMinutes);