using DBetter.Domain.Vehicles;
using DBetter.Domain.Vehicles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace DBetter.Infrastructure.Postgres;

public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new VehicleId(value));

        builder.Property(x => x.Identifier);
        
        builder.OwnsMany(x => x.CoachSequence, cb =>
        {
            cb.ToTable("VehicleCoaches");
            
            cb.WithOwner().HasForeignKey("VehicleId");
            
            cb.HasKey(nameof(Coach.Id), "VehicleId");

            cb.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => new CoachId(value));

            cb.Property(x => x.ConstructionType);

        }).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}