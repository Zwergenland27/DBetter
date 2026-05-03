using DBetter.Domain.Abstractions;
using DBetter.Domain.Consists.Coaches;
using DBetter.Domain.Consists.ValueObjects;
using DBetter.Domain.FixedConsists.ValueObjects;

namespace DBetter.Domain.Consists;

public class Consist: AggregateRoot<ConsistId>
{
    private List<ConsistCoach> _coaches;
    
    public FixedConsistId? FixedConsistId { get; private set; }
    public ConsistIdentifier Identifier { get; private set; }
    
    public IReadOnlyCollection<ConsistCoach> Coaches => _coaches.AsReadOnly();
    
    internal Consist(ConsistId id, FixedConsistId? fixedConsistId, ConsistIdentifier identifier, List<ConsistCoach> coaches) : base(id)
    {
        FixedConsistId = fixedConsistId;
        Identifier = identifier;
        _coaches = coaches;
    }
}