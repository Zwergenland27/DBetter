using DBetter.Domain.Abstractions;
using DBetter.Domain.Journey.ValueObjects;

namespace DBetter.Domain.Journey;

/// <summary>
/// Journey without any information
/// </summary>
/// <remarks>
/// Necessary to map journeyId to Bahn journey id
/// </remarks>
public class EmptyJourney : AggregateRoot<JourneyId>
{
    public BahnJourneyId BahnId { get; private init; }
    
    private EmptyJourney() : base(null!){}

    private EmptyJourney(
        JourneyId journeyId,
        BahnJourneyId bahnId) : base(journeyId)
    {
        BahnId = bahnId;
    }
}