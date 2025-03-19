using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.Connections.ValueObjects;

/// <summary>
/// Offer information for a connection
/// </summary>
/// <param name="ComfortClass">Class of the offer</param>
/// <param name="Price">Price value</param>
/// <param name="Currency">Currency</param>
/// <param name="SectionPrice">Indicates that the price only applies to a section</param>
public record Offer(ComfortClass ComfortClass, float Price, Currency Currency, bool SectionPrice);