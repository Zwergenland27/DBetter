namespace DBetter.Domain.Connections.ValueObjects;

public enum TransferState
{
    Waiting,
    Likely,
    AtRisk,
    Missed,
    NotWaiting,
    Cancelled
}