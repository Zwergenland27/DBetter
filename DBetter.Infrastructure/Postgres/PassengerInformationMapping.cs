using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class PassengerInformationMapping : IEntityTypeConfiguration<PassengerInformation>
{
    public void Configure(EntityTypeBuilder<PassengerInformation> builder)
    {
        builder.ToTable("PassengerInformation");
        
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Text)
            .IsUnique();
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new PassengerInformationId(value));

        builder.Property(x => x.Type);
        
        builder.Property(x => x.Priority);
        
        builder.Property(x => x.Text)
            .HasConversion(
                text => text.Value,
                value => new PassengerInformationText(value));
        
        builder.Property(x => x.Code)
            .HasConversion(
                code => code.Value,
                value => PassengerInformationCode.Create(value).Value);
    }
}