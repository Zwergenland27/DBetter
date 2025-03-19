using System.Text.Json.Serialization;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(WalkSection), typeDiscriminator: "walk")]
[JsonDerivedType(typeof(TransportSection), typeDiscriminator: "transport")]
public abstract class Section : Entity<SectionId>
{
    protected Section() : base(null!){}
    
    protected Section(SectionId id) : base(id){}
}