using DBetter.Domain.Abstractions;
using DBetter.Domain.Consists.ValueObjects;
using DBetter.Domain.FixedConsists.ValueObjects;

namespace DBetter.Domain.FixedConsists;

public class FixedConsist: AggregateRoot<ConsistId>
{
    public FixedConsistIdentifier Identifier { get; private set; }
    
    internal FixedConsist(ConsistId id, FixedConsistIdentifier identifier) : base(id)
    {
        Identifier = identifier;
    }
}