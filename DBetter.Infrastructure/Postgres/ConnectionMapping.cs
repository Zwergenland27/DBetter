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

        builder.HasIndex(x => x.TripId)
            .IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ConnectionId(value));
        
        builder.Property(x => x.TripId)
            .HasConversion(
                id => id.Value,
                value => new TripId(value));
        
        builder.Property(x => x.ContextId)
            .HasConversion(
                id => id.Value,
                value => new ContextId(value));

        builder.OwnsOne(x => x.Offer);

        builder.OwnsOne(x => x.Demand);
        
        builder.Property(x => x.BikeCarriage)
            .IsRequired(false);

        builder.OwnsMany(x => x.Messages, mb =>
            {
                mb.ToTable("ConnectionMessages");
                
                mb.WithOwner().HasForeignKey("ConnectionId");
                
                mb.Property<Guid>("Id");
                
                mb.HasKey("Id", "ConnectionId");

                mb.Property(x => x.Code);

                mb.Property(x => x.DefaultText);
            })
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Sections)
            .WithOne()
            .HasForeignKey("ConnectionId");

        builder.Navigation(x => x.Sections)
            .HasField("_sections");
    }
}