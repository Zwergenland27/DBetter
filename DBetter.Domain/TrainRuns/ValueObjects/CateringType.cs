namespace DBetter.Domain.TrainRuns.ValueObjects;

public enum CateringType
{
    None,
    Closed,
    Restaurant,
    Bistro,
    /// <summary>
    /// Food and beverages served at seat
    /// </summary>
    SeatServed,
    Snack
}