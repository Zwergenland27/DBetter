using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class TransferSection : Section
{
    private TransferSection(){}
    
    public TransferSection(SectionId id) : base(id, null) { }
}