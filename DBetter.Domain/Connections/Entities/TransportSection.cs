using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Journey.ValueObjects;
using JourneyId = DBetter.Domain.Connections.ValueObjects.JourneyId;

namespace DBetter.Domain.Connections.Entities;

public class TransportSection : Section
{
    private readonly List<SectionMessage> _messages = [];
    
    private readonly List<Stop> _stops = [];
    
    public Demand Demand { get; private set; }
    
    public IReadOnlyList<SectionMessage> Messages => _messages.AsReadOnly();
    
    public JourneyId JourneyId { get; private init; }
    
    public JourneyInformation Information { get; private init; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private TransportSection(){}

    public TransportSection(
        SectionId id,
        SectionIndex sectionIndex,
        Demand demand,
        List<SectionMessage> messages,
        JourneyId journeyId,
        JourneyInformation journeyInformation,
        List<Stop> stops) : base(id, sectionIndex)
    {
        Demand = demand;
        _messages = messages;
        JourneyId = journeyId;
        Information = journeyInformation;
        _stops = stops;
    }
}