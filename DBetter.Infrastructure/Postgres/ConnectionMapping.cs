using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;
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

        builder.Ignore(x => x.TripId);
        
        builder.Property(x => x.ContextId)
            .HasConversion(
                id => id.Value,
                value => new ContextId(value));

        builder.Ignore(x => x.Offer);

        builder.Ignore(x => x.Demand);

        builder.Ignore(x => x.BikeCarriage);

        builder.Ignore(x => x.Messages);

        builder.Ignore(x => x.Sections);

        builder.Ignore(x => x.RequestId);
    }
}