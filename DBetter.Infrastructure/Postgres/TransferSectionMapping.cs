using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class TransferSectionMapping : IEntityTypeConfiguration<TransferSection>
{

    public void Configure(EntityTypeBuilder<TransferSection> builder)
    {
        builder.ToTable("TransferSections");
    }
}