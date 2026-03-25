using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.TrainCompositions;

public class TrainCompositionMapping: IEntityTypeConfiguration<TrainCompositionPersistenceDto>
{
    public void Configure(EntityTypeBuilder<TrainCompositionPersistenceDto> builder)
    {
        builder.ToTable("TrainCompositions");
        
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Vehicles, vb =>
        {
            vb.ToTable("FormationVehicles");
            
            vb.WithOwner().HasForeignKey("TrainCompositionId");
            
            vb.HasKey("TrainCompositionId", nameof(FormationVehiclePersistenceDto.Id));
        });
    }
}