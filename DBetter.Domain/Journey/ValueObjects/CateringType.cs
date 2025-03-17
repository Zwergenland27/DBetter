namespace DBetter.Domain.Journey.ValueObjects;

public enum CateringType
{
    None,
    NoInfo,
    Restaurant,
    Bistro,
    /// <summary>
    /// Food and beverages served at seat
    /// </summary>
    SeatServed,
    Snack
}