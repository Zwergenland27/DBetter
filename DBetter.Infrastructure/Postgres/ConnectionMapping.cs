using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class ConnectionMapping : IEntityTypeConfiguration<ConnectionEntity>
{
    public void Configure(EntityTypeBuilder<ConnectionEntity> builder)
    {
        builder.ToTable("Connections");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ConnectionId(value));
        
        builder.Property(x => x.RequestId)
            .HasConversion(
                id => id.Value,
                value => new ConnectionRequestId(value));

        builder.Property(x => x.ContextId);
    }
}