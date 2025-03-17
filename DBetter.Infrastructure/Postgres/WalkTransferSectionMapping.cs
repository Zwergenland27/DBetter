using DBetter.Domain.Connections.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class WalkTransferSectionMapping : IEntityTypeConfiguration<WalkTransferSection>
{
    public void Configure(EntityTypeBuilder<WalkTransferSection> builder)
    {
        builder.ToTable("WalkTransferSections");

        builder.Property(x => x.Distance);

        builder.Property(x => x.WalkingMinutes);
    }
}