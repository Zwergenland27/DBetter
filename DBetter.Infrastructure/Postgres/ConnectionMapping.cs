using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
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

        builder.OwnsMany(x => x.Transfers, tb =>
        {
            tb.ToTable("Transfers");
            
            tb.WithOwner().HasForeignKey("ConnectionId");
            
            tb.HasKey("ConnectionId", nameof(Transfer.Id));

            tb.Property(x => x.Id)
                .HasConversion(
                    id => id.Index,
                    value => new TransferIndex(value))
                .ValueGeneratedNever();

            tb.OwnsOne(x => x.PreviousSubConnection, BuildSubConnection);
            tb.OwnsOne(x => x.FollowingSubConnection, BuildSubConnection);

        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private void BuildSubConnection(OwnedNavigationBuilder<Transfer, SubConnection> scb)
    {
        scb.Property(x => x.FromStationId)
            .HasConversion(
                stationId => stationId.Value,
                value => new StationId(value));

        scb.Property(x => x.DepartureTime);
                
        scb.Property(x => x.ToStationId)
            .HasConversion(
                stationId => stationId.Value,
                value => new StationId(value));
                
        scb.Property(x => x.ArrivalTime);

        scb.OwnsOne(x => x.OriginalMeansOfTransport, motb =>
        {
            motb.Ignore(mot => mot.AnySelected);
        });
    }
}