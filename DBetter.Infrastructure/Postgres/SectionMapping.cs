using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class SectionMapping : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new SectionId(value));
        
        builder.Property(x => x.SectionIndex)
            .HasConversion(
                index => index != null ? index.Value : (int?) null,
                value => value.HasValue ? new SectionIndex(value.Value) : null);
    }
}