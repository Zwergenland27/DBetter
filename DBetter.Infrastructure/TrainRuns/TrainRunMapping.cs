using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Entities;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.TrainRuns;

public class TrainRunMapping : IEntityTypeConfiguration<TrainRunPersistenceDto>
{
    public void Configure(EntityTypeBuilder<TrainRunPersistenceDto> builder)
    {
        builder.ToTable("TrainRuns");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => new {x.TrainCirculationId, x.OperatingDay})
            .IsUnique();

        builder.OwnsMany(x => x.PassengerInformation, pib =>
        {
            pib.ToTable("TrainRunPassengerInformation");

            pib.WithOwner().HasForeignKey("TrainRunId");

            pib.HasKey("TrainRunId", nameof(TrainRunPassengerInformationPersistenceDto.Id));

            pib.HasIndex(nameof(TrainRunPassengerInformation.Id), "TrainRunId")
                .IsUnique();
        });
    }
}