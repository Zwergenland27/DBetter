using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class ConnectionMapping : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.ToTable("Connections");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ConnectionId(value));

        builder.Property(x => x.ConnectionDate);

        builder.Property(x => x.ContextId)
            .HasConversion(
                ctxId => ctxId.Value,
                value => new ConnectionContextId(value));
    }
}