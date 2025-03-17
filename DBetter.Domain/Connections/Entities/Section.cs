using System.Text.Json.Serialization;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TransferSection), typeDiscriminator: "transfer")]
[JsonDerivedType(typeof(WalkTransferSection), typeDiscriminator: "walk")]
[JsonDerivedType(typeof(TransportSection), typeDiscriminator: "transport")]
public abstract class Section : Entity<SectionId>
{
    public SectionIndex? SectionIndex { get; set; }
    
    protected Section() : base(null!){}
    
    protected Section(SectionId id, SectionIndex? sectionIndex) : base(id)
    {
        SectionIndex = sectionIndex;
    }
}