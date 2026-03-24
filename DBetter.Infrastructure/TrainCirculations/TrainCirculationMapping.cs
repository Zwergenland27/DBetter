using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.TrainCirculations;

public class TrainCirculationMapping : IEntityTypeConfiguration<TrainCirculationPersistenceDto>
{
    public void Configure(EntityTypeBuilder<TrainCirculationPersistenceDto> builder)
    {
        builder.ToTable("TrainCirculations");
        
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Identifier)
            .IsUnique();
    }
}