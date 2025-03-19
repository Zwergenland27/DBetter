using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record ConnectionOptions(
    ComfortClass ComfortClass,
    int MaxTransfers,
    int MinTransferMinutes);